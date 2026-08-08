using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using PreOpeningPersistance.Context;
using PreOpeningApplication.Interfaces;
using PreOpeningApplication.DTOs;
using Domains.Entities;
using Shared;
using Shared.Global;

namespace PreOpeningPersistance.Repository
{
    /// <summary>
    /// Repository para gestionar operaciones de Pre-Apertura (Pre-Opening)
    /// Maneja carga de lotes, gestión en bodega y generación de formatos
    /// </summary>
    public class PreOpeningRepository : IPreOpeningRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly InformationSession _global;

        public PreOpeningRepository(ApplicationContext dbContext, InformationSession global)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        #region Lot Management Operations
        /// <summary>
        /// Carga un lote completo con toda su estructura (contenedores, pallets, cajas, detalles)
        /// </summary>
        public async Task<string> LoadLotAsync(LoadCkdLotRequestDto request)
        {
            try
            {
                //    foreach (var container in request.Containers)
                //    {
                //        foreach (var pallet in container.Pallets)
                //        {
                //            foreach (var box in pallet.Boxes)
                //            {
                //                foreach (var detail in box.ProductDetails)
                //                {
                //                    // Validar que el componente exista en la base de datos
                //                    var componentExists = await _dbContext.Components
                //                        .AnyAsync(c => c.ComponentCode == detail.ComponentCode);
                //                    if (!componentExists)
                //                    {
                //                        throw new NotFoundException($"Componente con código '{detail.ComponentCode}' no encontrado.");
                //                    }
                //                }
                //            }
                //        }
                //    }

                // Crear el lote
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                var ckdLot = new Lot
                {
                    LotCode = request.LotCode,
                    Product = request.Product,
                    ArrivalDate = request.ArrivalDate ?? DateTime.Now,
                    Status = LoteStatus.Received
                };

                _dbContext.Lot.Add(ckdLot);
                await _dbContext.SaveChangesAsync();

                // Procesar contenedores
                foreach (var containerRequest in request.Containers)
                {
                    var container = new Container
                    {
                        IdLot = ckdLot.IdLot,
                        ContainerNumber = containerRequest.ContainerCode,
                        Status = LoteStatus.Received,
                        StampNumber = containerRequest.StampNumber ?? string.Empty
                    };

                    _dbContext.Set<Container>().Add(container);
                    await _dbContext.SaveChangesAsync();

                    // Procesar pallets
                    foreach (var palletRequest in containerRequest.Pallets)
                    {
                        var pallet = new Pallet
                        {
                            IdContainer = container.IdContainer,
                            PalletNumber = palletRequest.PalletCode,
                            Status = LoteStatus.Received,
                        };

                        _dbContext.Set<Pallet>().Add(pallet);
                        await _dbContext.SaveChangesAsync();

                        // Procesar cajas
                        foreach (var boxRequest in palletRequest.Boxes)
                        {
                            var box = new Box
                            {
                                IdPallet = pallet.IdPallet,
                                BoxNumber = boxRequest.BoxCode,
                                Status = LoteStatus.Received
                            };

                            _dbContext.Set<Box>().Add(box);
                            await _dbContext.SaveChangesAsync();

                            var boxComponentCodes = boxRequest.ProductDetails
                                .Select(d => d.ComponentCode)
                                .Distinct()
                                .ToList();
                            
                            var components = await _dbContext.Component
                                .Where(c => boxComponentCodes.Contains(c.PartCode))
                                .Select(c => new { c.IdComponent, c.PartCode })
                                .ToDictionaryAsync(c => c.PartCode, c => c.IdComponent);

                            // Procesar detalles de producto
                            foreach (var detailRequest in boxRequest.ProductDetails)
                            {

                                var detail = new BoxProductDetail
                                {
                                    IdBox = box.IdBox,
                                    IdComponent = components.GetValueOrDefault(detailRequest.ComponentCode),
                                    TotalQuantity = detailRequest.TotalQuantity,
                                    PackingQuantity = detailRequest.PackingQuantity,
                                };

                                _dbContext.Set<BoxProductDetail>().Add(detail);
                            }

                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
                await transaction.CommitAsync();
                return "Lote cargado exitosamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al cargar el lote: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un lote completo con todos sus detalles anidados
        /// </summary>
        public async Task<CkdLotFullDetailsDto> GetCkdLotWithAllDetailsAsync(int ckdLotId)
        {
            try
            {
                if (ckdLotId <= 0)
                    throw new ApiException("IdLot inválido");

                var ckdLot = await _dbContext.Set<Lot>()
                    .AsNoTracking()
                    .Include(c => c.Container)
                        .ThenInclude(con => con.Pallet)
                            .ThenInclude(p => p.Box)
                                .ThenInclude(b => b.BoxProductDetail)
                                    .ThenInclude(bd => bd.IdComponentNavigation)
                    .Include(c => c.Container)
                        .ThenInclude(con => con.Pallet)
                            .ThenInclude(p => p.IdUserNavigation)
                    .Include(c => c.Container)
                        .ThenInclude(con => con.IdStoreNavigation)
                    .FirstOrDefaultAsync(c => c.IdLot == ckdLotId);

                if (ckdLot == null)
                    throw new ApiException($"CKD Lot con ID {ckdLotId} no encontrado");

                var dto = new CkdLotFullDetailsDto
                {
                    IdLot = ckdLot.IdLot,
                    LotCode = ckdLot.LotCode,
                    Product = ckdLot.Product,
                    ArrivalDate = ckdLot.ArrivalDate,
                    Status = ckdLot.Status,
                    Containers = ckdLot.Container.Select(container => new ContainerFullDetailsDto
                    {
                        IdContainer = container.IdContainer,
                        ContainerCode = container.ContainerNumber,
                        Status = container.Status,
                        IdStore = container.IdStore,
                        StoreName = container.IdStoreNavigation?.StoreName,
                        Pallets = container.Pallet.Select(pallet => new PalletFullDetailsDto
                        {
                            IdPallet = pallet.IdPallet,
                            PalletCode = pallet.PalletNumber,
                            Status = pallet.Status,
                            Content = pallet.Content,
                            Description = pallet.Description,
                            Claim = pallet.NeedClaim,
                            IdUserInCharge = pallet.IdUser.GetValueOrDefault(),
                            UserInChargeName = $"{pallet.IdUserNavigation?.FirstName} {pallet.IdUserNavigation?.LastName}",
                            Boxes = pallet.Box.Select(box => new BoxFullDetailsDto
                            {
                                IdBox = box.IdBox,
                                BoxCode = box.BoxNumber,
                                Status = box.Status,
                                ProductDetails = box.BoxProductDetail.Select(detail => new BoxProductDetailFullDto
                                {
                                    IdBoxProductDetail = detail.IdBoxProductDetail,
                                    IdComponent = detail.IdComponent,
                                    ComponentCode = detail.IdComponentNavigation?.PartCode,
                                    ComponentDescription = detail.IdComponentNavigation?.Description,
                                    ExpectedQuantity = detail.TotalQuantity,
                                    RevisedQuantity = detail.RevisedQuantity,
                                    UpdateDate = detail.UpdateDate
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al cargar lotes: {ex}");
            }
        }

        /// <summary>
        /// Pone contenedores en bodega y actualiza estado en cascada
        /// </summary>
        public async Task<string> StoreContainerAsync(StoreContainerRequestDto request)
        {
            try
            {
                if (request == null || request.IdContainer <= 0 || request.IdStore <= 0)
                    throw new ApiException("Datos inválidos");

                var container = await _dbContext.Set<Container>()
                    .Include(c => c.IdLotNavigation)
                    .Include(c => c.IdStoreNavigation)
                    .FirstOrDefaultAsync(c => c.IdContainer == request.IdContainer);

                if (container == null)
                    throw new ApiException($"Contenedor con ID {request.IdContainer} no encontrado");

                var store = await _dbContext.Set<Store>()
                    .FirstOrDefaultAsync(s => s.IdStore == request.IdStore);

                if (store == null)
                    throw new ApiException($"Bodega con ID {request.IdStore} no encontrada");

                // Determinar el estado basado en el nombre de la bodega
                string newStatus = (store.StoreName != null && store.StoreName.Equals("ESTACIÓN", StringComparison.OrdinalIgnoreCase)) 
                    ? LoteStatus.Located 
                    : LoteStatus.Stocked;

                // Actualizar contenedor
                container.IdStore = request.IdStore;
                container.Status = newStatus;

                _dbContext.Set<Container>().Update(container);
                await _dbContext.SaveChangesAsync();

                // Actualizar estado en cascada hacia abajo (pallets, cajas, detalles)
                await UpdateCascadeDownFromContainerAsync(container.IdContainer, newStatus);

                // Actualizar estado en cascada hacia arriba (lote)
                await UpdateCascadeUpFromContainerAsync(container.IdContainer);
                return "Contenedor almacenado exitosamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al ubicar contenedor");
            }
        }

        #endregion

        #region Format Generation Operations

        /// <summary>
        /// Genera formato de descarga de contenedores (solo bodega ESTACIÓN)
        /// </summary>
        public async Task<ContainerUnloadingFormatDto> GenerateContainerUnloadingFormatAsync(int ckdLotId)
        {
            try
            {
                var format = await (from l in _dbContext.Lot
                                   where l.IdLot == ckdLotId

                                   select new ContainerUnloadingFormatDto
                                   {
                                       CkdLotId = ckdLotId,
                                       LotCode = l.LotCode,
                                       Product = l.Product,
                                       Containers = (from c in _dbContext.Container
                                                    join s in _dbContext.Store on c.IdStore equals s.IdStore
                                                    where c.IdLot == ckdLotId && s.StoreName == "ESTACIÓN"
                                                    select new ContainerUnloadingDetailDto
                                       {
                                           IdContainer = c.IdContainer,
                                           ContainerCode = c.ContainerNumber,
                                           StoreName = "ESTACIÓN",
                                           UnloadingDate = c.DisembarkationDate.GetValueOrDefault().ToString("yyyy-MM-dd") ?? string.Empty,
                                           PalletCount = c.Pallet.Count,
                                           Pallets = (from p in _dbContext.Pallet
                                                      where p.IdContainer == c.IdContainer
                                                      join u in _dbContext.Users on p.IdUser equals u.IdUser into ug
                                                      from u in ug.DefaultIfEmpty()
                                                      select new PalletUnloadingDetailDto
                                                      {
                                                          IdPallet = p.IdPallet,
                                                          PalletCode = p.PalletNumber,
                                                          Content = p.Content,
                                                          Description = p.Description,
                                                          Claim = p.NeedClaim,
                                                          UserName = u != null ? u.FirstName + " " + u.LastName : string.Empty,
                                                          BoxCount = p.Box.Count
                                                      }).ToList()
                                       }).ToList()
                                   }).FirstOrDefaultAsync() ?? throw new ApiException($"Formato no encontrado para el lote {ckdLotId}");

                return format;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al cargar formato");
            }
        }

        /// <summary>
        /// Genera formato de listado de desempaque con distribución de componentes por estación
        /// Basado en la entidad Recipe que guía la distribución de componentes a estaciones
        /// La cantidad total se obtiene de Recipe.Total y además se mapea CantCar.
        /// </summary>
        public async Task<UnpackingFormatDto> GenerateUnpackingFormatAsync(int ckdLotId)
        {
            try
            {
                var ckdLot = await _dbContext.Set<Lot>()
                    .AsNoTracking()
                    .Include(c => c.Container)
                        .ThenInclude(con => con.Pallet)
                            .ThenInclude(p => p.Box)
                                .ThenInclude(b => b.BoxProductDetail)
                                    .ThenInclude(bd => bd.IdComponentNavigation)
                    .FirstOrDefaultAsync(c => c.IdLot == ckdLotId);

                if (ckdLot == null)
                    throw new ApiException($"CKD Lot con ID {ckdLotId} no encontrado");

                var model = await _dbContext.Set<Model>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ModelName == ckdLot.Product || m.ModelCode == ckdLot.Product);

                if (model == null)
                    throw new ApiException($"Modelo correspondiente al producto '{ckdLot.Product}' no encontrado");

                // Obtener todas las recipes disponibles para mapeo de componentes a estaciones según el modelo
                var recipes = await _dbContext.Set<Recipe>()
                    .AsNoTracking()
                    .Include(r => r.IdComponentNavigation)
                    .Include(r => r.IdStationNavigation)
                    .Where(r => r.IdModel == model.IdModel)
                    .ToListAsync();

                if (!recipes.Any())
                    throw new ApiException($"No hay recipes disponibles para la distribución del modelo '{ckdLot.Product}'");

                // Crear mapa de requerimientos restantes por componente y estación
                var remainingRequirements = new Dictionary<int, Dictionary<int, int>>();
                foreach (var r in recipes.GroupBy(x => new { x.IdComponent, x.IdStation }).Select(g => g.First()))
                {
                    if (r.Total.HasValue && r.Total.Value > 0)
                    {
                        if (!remainingRequirements.ContainsKey(r.IdComponent))
                            remainingRequirements[r.IdComponent] = new Dictionary<int, int>();
                        
                        remainingRequirements[r.IdComponent][r.IdStation] = r.Total.Value;
                    }
                }

                var format = new UnpackingFormatDto
                {
                    CkdLotId = ckdLotId,
                    LotCode = ckdLot.LotCode,
                    Product = ckdLot.Product,
                    Pallets = new List<PalletUnpackingDetailDto>()
                };

                // Iterar secuencialmente para mantener el estado de remainingRequirements
                foreach (var container in ckdLot.Container)
                {
                    foreach (var pallet in container.Pallet)
                    {
                        var palletDto = new PalletUnpackingDetailDto
                        {
                            IdPallet = pallet.IdPallet,
                            PalletCode = pallet.PalletNumber,
                            Boxes = new List<BoxDistributionDetailDto>()
                        };

                        foreach (var box in pallet.Box)
                        {
                            var boxDto = new BoxDistributionDetailDto
                            {
                                IdBox = box.IdBox,
                                BoxCode = box.BoxNumber,
                                BoxStatus = box.Status,
                                ComponentDistributions = new List<ComponentDistributionDto>()
                            };

                            foreach (var detail in box.BoxProductDetail)
                            {
                                var componentDist = new ComponentDistributionDto
                                {
                                    IdBoxProductDetail = detail.IdBoxProductDetail,
                                    IdComponent = detail.IdComponent,
                                    ComponentCode = detail.IdComponentNavigation?.PartCode,
                                    ComponentName = detail.IdComponentNavigation?.Description,
                                    ExpectedQuantity = detail.TotalQuantity,
                                    RevisedQuantity = detail.RevisedQuantity,
                                    StationDistributions = new List<ComponentStationDistributionDto>()
                                };

                                int availableQuantity = detail.TotalQuantity;

                                if (remainingRequirements.TryGetValue(detail.IdComponent, out var stationReqs))
                                {
                                    // Usamos un loop sobre las llaves para poder modificar el diccionario
                                    foreach (var idStation in stationReqs.Keys.ToList())
                                    {
                                        int required = stationReqs[idStation];
                                        
                                        if (required > 0 && availableQuantity > 0)
                                        {
                                            int allocated = Math.Min(required, availableQuantity);
                                            
                                            // Descontar la cantidad
                                            availableQuantity -= allocated;
                                            stationReqs[idStation] -= allocated;
                                            
                                            // Buscar la recipe original para mapear el CantCar y nombre
                                            var recipe = recipes.FirstOrDefault(r => r.IdComponent == detail.IdComponent && r.IdStation == idStation);

                                            componentDist.StationDistributions.Add(new ComponentStationDistributionDto
                                            {
                                                IdStation = idStation,
                                                StationName = recipe?.IdStationNavigation?.Name ?? "Unknown",
                                                Quantity = allocated,
                                                CantCar = recipe?.CantCar ?? 0
                                            });
                                        }
                                    }
                                }

                                // Descartar los que no fueron distribuidos ("descartalos entre comillas")
                                if (componentDist.StationDistributions.Count > 0)
                                {
                                    componentDist.ExpectedQuantity = componentDist.StationDistributions.Sum(s => s.Quantity);
                                    boxDto.ComponentDistributions.Add(componentDist);
                                }
                            }

                            // Solo incluir cajas que tengan al menos una distribución válida
                            if (boxDto.ComponentDistributions.Count > 0)
                            {
                                palletDto.Boxes.Add(boxDto);
                            }
                        }
                        
                        // Solo incluir pallets que tengan cajas válidas
                        if (palletDto.Boxes.Count > 0)
                        {
                            format.Pallets.Add(palletDto);
                        }
                    }
                }

                return format;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al generar formato de distribución: {ex.Message}");
            }
        }

        #endregion

        #region Unpacking Operations

        /// <summary>
        /// Registra un pallet en desempaque y actualiza estado en cascada
        /// </summary>
        public async Task<string> StorePalletAsync(StorePalletRequestDto request)
        {
            try
            {
                var pallet = await _dbContext.Set<Pallet>()
    .Include(p => p.IdContainerNavigation)
        .ThenInclude(c => c.IdLotNavigation)
    .FirstOrDefaultAsync(p => p.IdPallet == request.IdPallet) ?? throw new ApiException($"Pallet con ID {request.IdPallet} no encontrado");

                // Actualizar pallet
                pallet.Content = request.Content ?? pallet.Content;
                pallet.Description = request.Description ?? pallet.Description;
                pallet.NeedClaim = request.Claim ?? pallet.NeedClaim;
                pallet.Status = LoteStatus.InProcess;
                pallet.IdUser = _global.UserId;

                _dbContext.Set<Pallet>().Update(pallet);
                await _dbContext.SaveChangesAsync();

                // Actualizar estado en cascada hacia abajo (cajas, detalles)
                await UpdateCascadeDownFromPalletAsync(pallet.IdPallet, LoteStatus.InProcess);

                // Actualizar estado en cascada hacia arriba (contenedor, lote)
                await UpdateCascadeUpFromPalletAsync(pallet.IdPallet);

                return "Pallet registrado correctamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al registrar pallet: {ex}");
            }
        }

        /// <summary>
        /// Crea registros en tabla cardboard para desempaque basado en la entidad Recipe
        /// Genera la distribución usando GenerateUnpackingFormatAsync y crea un Cardboard por cada distribución
        /// Cada Cardboard relaciona un BoxProductDetail con una Recipe (que define la estación)
        /// Cambia todos los elementos del lote a estado "POR_PROCESAR"
        /// </summary>
        public async Task<string> CreateCardboardsAsync(CreateLabeledBoxesRequestDto request)
        {
            try
            {
                // Generar distribución automática usando GenerateUnpackingFormatAsync
                var unpackingFormat = await GenerateUnpackingFormatAsync(request.CkdLotId);

                var ckdLot = await _dbContext.Set<Lot>()
                    .Include(l => l.Container)
                        .ThenInclude(c => c.Pallet)
                            .ThenInclude(p => p.Box)
                                .ThenInclude(b => b.BoxProductDetail)
                    .FirstOrDefaultAsync(c => c.IdLot == request.CkdLotId);

                if (ckdLot == null)
                    throw new ApiException($"CKD Lot con ID {request.CkdLotId} no encontrado");

                var model = await _dbContext.Set<Model>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ModelName == ckdLot.Product || m.ModelCode == ckdLot.Product);

                if (model == null)
                    throw new ApiException($"Modelo correspondiente al producto '{ckdLot.Product}' no encontrado");

                // Obtener todas las recipes para mapeo de estación -> recipe según el modelo
                var recipes = await _dbContext.Set<Recipe>()
                    .AsNoTracking()
                    .Where(r => r.IdModel == model.IdModel)
                    .ToListAsync();

                var createdResponses = new List<LabeledBoxCreatedResponseDto>();

                // Crear un Cardboard por cada distribución de estación
                foreach (var pallet in unpackingFormat.Pallets)
                {
                    foreach (var boxDistribution in pallet.Boxes)
                    {
                        foreach (var componentDistribution in boxDistribution.ComponentDistributions)
                        {
                            foreach (var stationDist in componentDistribution.StationDistributions)
                            {
                                // Validar que hay estación asignada
                                if (stationDist.IdStation <= 0 || stationDist.Quantity <= 0)
                                    continue;

                                var station = await _dbContext.Set<WorkStation>()
                                    .FirstOrDefaultAsync(s => s.IdStation == stationDist.IdStation);

                                if (station == null)
                                    throw new ApiException($"Estación con ID {stationDist.IdStation} no encontrada");

                                // Buscar la recipe que corresponde a este componente y estación
                                var recipe = recipes.FirstOrDefault(r =>
                                    r.IdComponent == componentDistribution.IdComponent &&
                                    r.IdStation == stationDist.IdStation);

                                if (recipe == null)
                                    throw new ApiException($"Recipe no encontrada para componente {componentDistribution.IdComponent} en estación {stationDist.IdStation}");

                                // IdBox ahora es FK a BoxProductDetail.IdBoxProductDetail
                                var cardboard = new Cardboard
                                {
                                    IdBox = componentDistribution.IdBoxProductDetail,
                                    IdRecipe = recipe.IdRecipe,
                                    Status = LoteStatus.ToProcess,
                                    OpeningDate = DateTime.Now,
                                    IdUser = _global.UserId
                                };

                                _dbContext.Set<Cardboard>().Add(cardboard);
                                await _dbContext.SaveChangesAsync();

                                createdResponses.Add(new LabeledBoxCreatedResponseDto
                                {
                                    IdLabeledBox = cardboard.IdCardboard,
                                    IdBox = cardboard.IdBox,
                                    BoxCode = boxDistribution.BoxCode,
                                    IdComponent = componentDistribution.IdComponent,
                                    ComponentCode = componentDistribution.ComponentCode,
                                    ComponentName = componentDistribution.ComponentName,
                                    IdStation = stationDist.IdStation,
                                    StationName = station.Name,
                                    Quantity = stationDist.Quantity,
                                    SupplyStatus = LoteStatus.ToProcess
                                });
                            }
                        }
                    }
                }

                // Cambiar TODO el lote a "POR_PROCESAR"
                // Actualizar todos los contenedores
                foreach (var container in ckdLot.Container)
                {
                    container.Status = LoteStatus.ToProcess;
                    _dbContext.Set<Container>().Update(container);

                    // Actualizar todos los pallets del contenedor
                    foreach (var pallet in container.Pallet)
                    {
                        pallet.Status = LoteStatus.ToProcess;
                        _dbContext.Set<Pallet>().Update(pallet);

                        // Actualizar todas las cajas del pallet
                        foreach (var box in pallet.Box)
                        {
                            box.Status = LoteStatus.ToProcess;
                            _dbContext.Set<Box>().Update(box);

                            // Actualizar todos los detalles de la caja
                            foreach (var detail in box.BoxProductDetail)
                            {
                                detail.UpdateDate = DateTime.Now;
                                _dbContext.Set<BoxProductDetail>().Update(detail);
                            }
                        }
                    }
                }

                // Actualizar el lote a "POR_PROCESAR"
                ckdLot.Status = LoteStatus.ToProcess;
                _dbContext.Set<Lot>().Update(ckdLot);

                await _dbContext.SaveChangesAsync();
                return "Cardboards creados correctamente basado en distribucion de recipes";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al crear cardboards: {ex.Message}");
            }
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Obtiene todos los pallets de un contenedor específico
        /// </summary>
        public async Task<PalletsByContainerResponseDto> GetPalletsByContainerAsync(int idContainer)
        {
            var container = await _dbContext.Set<Container>()
                .AsNoTracking()
                .Include(c => c.Pallet)
                    .ThenInclude(p => p.IdUserNavigation)
                .Include(c => c.IdStoreNavigation)
                .Include(c => c.IdLotNavigation)
                .FirstOrDefaultAsync(c => c.IdContainer == idContainer) ?? throw new ApiException($"Contenedor con ID {idContainer} no encontrado");

            var response = new PalletsByContainerResponseDto
            {
                IdContainer = container.IdContainer,
                ContainerCode = container.ContainerNumber,
                IdLot = container.IdLot,
                Status = container.Status,
                IdStore = container.IdStore.GetValueOrDefault(),
                StoreName = container.IdStoreNavigation?.StoreName,
                Pallets = container.Pallet.Select(pallet => new PalletDetailDto
                {
                    IdPallet = pallet.IdPallet,
                    PalletCode = pallet.PalletNumber,
                    Content = pallet.Content,
                    Description = pallet.Description,
                    Status = pallet.Status,
                    Claim = pallet.NeedClaim.GetValueOrDefault(),
                    IdUserInCharge = pallet.IdUser.GetValueOrDefault(),
                    UserInChargeName = $"{pallet.IdUserNavigation?.FirstName} {pallet.IdUserNavigation?.LastName}".Trim()
                }).ToList()
            };

            return response;
        }

        /// <summary>
        /// Obtiene todos los contenedores con sus pallets de un CKD Lot
        /// </summary>
        public async Task<ContainersWithPalletsByCkdResponseDto> GetContainersWithPalletsByCkdAsync(int idCkdLot)
        {
            if (idCkdLot <= 0)
                throw new ApiException("IdCkdLot inválido");

            var ckdLot = await _dbContext.Set<Lot>()
                .AsNoTracking()
                .Include(c => c.Container)
                    .ThenInclude(con => con.Pallet)
                        .ThenInclude(p => p.IdUserNavigation)
                .Include(c => c.Container)
                    .ThenInclude(con => con.IdStoreNavigation)
                .FirstOrDefaultAsync(c => c.IdLot == idCkdLot);

            if (ckdLot == null)
                throw new ApiException($"CKD Lot con ID {idCkdLot} no encontrado");

            var response = new ContainersWithPalletsByCkdResponseDto
            {
                IdLot = ckdLot.IdLot,
                LotCode = ckdLot.LotCode,
                Product = ckdLot.Product,
                ArrivalDate = ckdLot.ArrivalDate.GetValueOrDefault(),
                Status = ckdLot.Status,
                Containers = ckdLot.Container.Select(container => new ContainerWithPalletsDetailDto
                {
                    IdContainer = container.IdContainer,
                    ContainerCode = container.ContainerNumber,
                    Status = container.Status,
                    IdStore = container.IdStore.GetValueOrDefault(),
                    StoreName = container.IdStoreNavigation?.StoreName,
                    Pallets = container.Pallet.Select(pallet => new PalletDetailDto
                    {
                        IdPallet = pallet.IdPallet,
                        PalletCode = pallet.PalletNumber,
                        Content = pallet.Content,
                        Description = pallet.Description,
                        Status = pallet.Status,
                        Claim = pallet.NeedClaim.GetValueOrDefault(),
                        IdUserInCharge = pallet.IdUser.GetValueOrDefault(),
                        UserInChargeName = $"{pallet.IdUserNavigation?.FirstName} {pallet.IdUserNavigation?.LastName}".Trim()
                    }).ToList()
                }).ToList()
            };

            return response;
        }

        /// <summary>
        /// Obtiene lista simplificada de todos los lotes CKD
        /// Retorna solo IdLot, LotCode y Product
        /// </summary>
        public async Task<CkdLotSimpleListResponseDto> GetAllCkdLotsAsync(string? status = null)
        {
            var query = _dbContext.Set<Lot>().AsNoTracking();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(l => l.Status == status);
            }

            var lots = await query
                .Select(c => new CkdLotSimpleDto
                {
                    IdLot = c.IdLot,
                    LotCode = c.LotCode,
                    Product = c.Product
                })
                .OrderByDescending(c => c.IdLot)
                .ToListAsync();

            return new CkdLotSimpleListResponseDto
            {
                Lots = lots
            };
        }

        /// <summary>
        /// Obtiene lista simplificada de todos los contenedores de un lote CKD
        /// Retorna solo IdContainer y ContainerCode
        /// </summary>
        public async Task<ContainerSimpleListResponseDto> GetAllContainersByCkdLotAsync(int idCkdLot)
        {
            if (idCkdLot <= 0)
                throw new ApiException("IdCkdLot inválido");

            var ckdLot = await _dbContext.Set<Lot>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdLot == idCkdLot);

            if (ckdLot == null)
                throw new ApiException($"CKD Lot con ID {idCkdLot} no encontrado");

            var containers = await _dbContext.Set<Container>()
                .AsNoTracking()
                .Where(c => c.IdLot == idCkdLot)
                .Select(c => new ContainerSimpleDto
                {
                    IdContainer = c.IdContainer,
                    ContainerCode = c.ContainerNumber,
                    IdStore = c.IdStore.GetValueOrDefault(),

                })
                .OrderByDescending(c => c.IdContainer)
                .ToListAsync();

            return new ContainerSimpleListResponseDto
            {
                Containers = containers
            };
        }

        /// <summary>
        /// Obtiene lista simplificada de todos los pallets de un contenedor
        /// Retorna solo IdPallet, PalletCode y Content
        /// </summary>
        public async Task<PalletSimpleListResponseDto> GetAllPalletsByContainerAsync(int idContainer)
        {
            if (idContainer <= 0)
                throw new ApiException("IdContainer inválido");

            var container = await _dbContext.Set<Container>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdContainer == idContainer);

            if (container == null)
                throw new ApiException($"Contenedor con ID {idContainer} no encontrado");

            var pallets = await _dbContext.Set<Pallet>()
                .AsNoTracking()
                .Where(p => p.IdContainer == idContainer)
                .Select(p => new PalletSimpleDto
                {
                    IdPallet = p.IdPallet,
                    PalletCode = p.PalletNumber,
                    Content = p.Content
                })
                .OrderByDescending(p => p.IdPallet)
                .ToListAsync();

            return new PalletSimpleListResponseDto
            {
                Pallets = pallets
            };
        }

        #endregion

        /// <summary>
        /// Actualiza el estado hacia abajo (en todos los hijos) desde un contenedor
        /// Si se actualiza un contenedor a por procesar, pasan todos sus pallets, cajas y detalles al mismo estado
        /// </summary>
        private async Task UpdateCascadeDownFromContainerAsync(int idContainer, string newStatus)
        {
            var container = await _dbContext.Set<Container>()
                .Include(c => c.Pallet)
                    .ThenInclude(p => p.Box)
                        .ThenInclude(b => b.BoxProductDetail)
                .FirstOrDefaultAsync(c => c.IdContainer == idContainer);

            if (container == null)
                return;

            // Actualizar todos los pallets del contenedor
            foreach (var pallet in container.Pallet)
            {
                pallet.Status = newStatus;
                _dbContext.Set<Pallet>().Update(pallet);

                // Actualizar todas las cajas del pallet
                foreach (var box in pallet.Box)
                {
                    box.Status = newStatus;
                    _dbContext.Set<Box>().Update(box);

                    // Actualizar todos los detalles de la caja
                    foreach (var detail in box.BoxProductDetail)
                    {
                        detail.UpdateDate = DateTime.Now;
                        _dbContext.Set<BoxProductDetail>().Update(detail);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza el estado hacia abajo (en todos los hijos) desde un pallet
        /// Si se actualiza un pallet a en proceso, se actualizan todos sus hijos (cajas y detalles) a ese estado
        /// </summary>
        private async Task UpdateCascadeDownFromPalletAsync(int idPallet, string newStatus)
        {
            var pallet = await _dbContext.Set<Pallet>()
                .Include(p => p.Box)
                    .ThenInclude(b => b.BoxProductDetail)
                .FirstOrDefaultAsync(p => p.IdPallet == idPallet);

            if (pallet == null)
                return;

            // Actualizar todas las cajas del pallet
            foreach (var box in pallet.Box)
            {
                box.Status = newStatus;
                _dbContext.Set<Box>().Update(box);

                // Actualizar todos los detalles de la caja
                foreach (var detail in box.BoxProductDetail)
                {
                    detail.UpdateDate = DateTime.Now;
                    _dbContext.Set<BoxProductDetail>().Update(detail);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza el estado en cascada hacia arriba desde un contenedor
        /// Si todos los contenedores de un lote están en el mismo estado, actualiza el lote
        /// </summary>
        private async Task UpdateCascadeUpFromContainerAsync(int idContainer)
        {
            var container = await _dbContext.Set<Container>()
                .Include(c => c.IdLotNavigation)
                    .ThenInclude(l => l.Container)
                        .ThenInclude(con => con.Pallet)
                .FirstOrDefaultAsync(c => c.IdContainer == idContainer);

            if (container == null || container.IdLotNavigation == null)
                return;

            var ckdLot = container.IdLotNavigation;

            // Verificar si todos los contenedores tienen el mismo estado
            if (ckdLot.Container.Any())
            {
                var allContainerStatuses = ckdLot.Container.Select(c => c.Status).Distinct().ToList();

                // Si todos los contenedores tienen el mismo estado, actualizar lote
                if (allContainerStatuses.Count == 1)
                {
                    var newStatus = allContainerStatuses.First();
                    if (ckdLot.Status != newStatus)
                    {
                        ckdLot.Status = newStatus;
                        _dbContext.Set<Lot>().Update(ckdLot);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else if (allContainerStatuses.All(s => s == LoteStatus.Located || s == LoteStatus.Stocked))
                {
                    if (ckdLot.Status != LoteStatus.Located)
                    {
                        ckdLot.Status = LoteStatus.Located;
                        _dbContext.Set<Lot>().Update(ckdLot);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza el estado en cascada hacia arriba desde un pallet
        /// Si todos los pallets de un contenedor están en el mismo estado, actualiza el contenedor
        /// Si todos los contenedores de un lote están en el mismo estado, actualiza el lote
        /// </summary>
        private async Task UpdateCascadeUpFromPalletAsync(int idPallet)
        {
            var pallet = await _dbContext.Set<Pallet>()
                .Include(p => p.IdContainerNavigation)
                    .ThenInclude(c => c.IdLotNavigation)
                        .ThenInclude(l => l.Container)
                            .ThenInclude(con => con.Pallet)
                .FirstOrDefaultAsync(p => p.IdPallet == idPallet);

            if (pallet == null || pallet.IdContainerNavigation == null)
                return;

            var container = pallet.IdContainerNavigation;
            container.DisembarkationDate = DateTime.Now;
            _dbContext.Set<Container>().Update(container);
            await _dbContext.SaveChangesAsync();

            // Verificar si todos los pallets del contenedor tienen el mismo estado
            if (container.Pallet.Any())
            {
                var allPalletStatuses = container.Pallet.Select(p => p.Status).Distinct().ToList();

                // Si todos los pallets tienen el mismo estado, actualizar contenedor
                if (allPalletStatuses.Count == 1)
                {
                    var newStatus = allPalletStatuses.First();
                    if (container.Status != newStatus)
                    {
                        container.Status = newStatus;
                        _dbContext.Set<Container>().Update(container);
                        await _dbContext.SaveChangesAsync();

                        // Ahora actualizar hacia arriba el lote
                        if (container.IdLotNavigation != null)
                        {
                            var ckdLot = container.IdLotNavigation;
                            var allContainerStatuses = ckdLot.Container.Select(c => c.Status).Distinct().ToList();

                            // Si todos los contenedores tienen el mismo estado, actualizar lote
                            if (allContainerStatuses.Count == 1)
                            {
                                var lotNewStatus = allContainerStatuses.First();
                                if (ckdLot.Status != lotNewStatus)
                                {
                                    ckdLot.Status = lotNewStatus;
                                    _dbContext.Set<Lot>().Update(ckdLot);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            else if (allContainerStatuses.All(s => s == LoteStatus.Located || s == LoteStatus.Stocked))
                            {
                                if (ckdLot.Status != LoteStatus.Located)
                                {
                                    ckdLot.Status = LoteStatus.Located;
                                    _dbContext.Set<Lot>().Update(ckdLot);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza el estado en cascada hacia arriba desde un lote
        /// Solo cambia estado si todos los hijos tienen el mismo estado nuevo
        /// </summary>
        private async Task UpdateCascadeStatusAsync(int ckdLotId)
        {
            var ckdLot = await _dbContext.Set<Lot>()
                .Include(c => c.Container)
                    .ThenInclude(con => con.Pallet)
                        .ThenInclude(p => p.Box)
                .FirstOrDefaultAsync(c => c.IdLot == ckdLotId);

            if (ckdLot == null)
                return;

            // Actualizar contenedores basado en pallets
            foreach (var container in ckdLot.Container)
            {
                if (container.Pallet.Any())
                {
                    var allPalletsStatuses = container.Pallet.Select(p => p.Status).Distinct().ToList();

                    // Si todos los pallets tienen el mismo estado, actualizar contenedor
                    if (allPalletsStatuses.Count == 1)
                    {
                        var newStatus = allPalletsStatuses.First();
                        if (container.Status != newStatus)
                        {
                            container.Status = newStatus;
                            _dbContext.Set<Container>().Update(container);
                        }
                    }
                    else if (allPalletsStatuses.Contains(LoteStatus.ToProcess))
                    {
                        // Si hay al menos uno en POR_PROCESAR, cambiar a POR_PROCESAR
                        if (container.Status != LoteStatus.ToProcess)
                        {
                            container.Status = LoteStatus.ToProcess;
                            _dbContext.Set<Container>().Update(container);
                        }
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            // Actualizar lote basado en contenedores
            if (ckdLot.Container.Any())
            {
                var allContainerStatuses = ckdLot.Container.Select(c => c.Status).Distinct().ToList();

                // Si todos los contenedores tienen el mismo estado, actualizar lote
                if (allContainerStatuses.Count == 1)
                {
                    var newStatus = allContainerStatuses.First();
                    if (ckdLot.Status != newStatus)
                    {
                        ckdLot.Status = newStatus;
                        _dbContext.Set<Lot>().Update(ckdLot);
                    }
                }
                else if (allContainerStatuses.All(s => s == LoteStatus.Located || s == LoteStatus.Stocked))
                {
                    if (ckdLot.Status != LoteStatus.Located)
                    {
                        ckdLot.Status = LoteStatus.Located;
                        _dbContext.Set<Lot>().Update(ckdLot);
                    }
                }
                else if (allContainerStatuses.Contains(LoteStatus.ToProcess))
                {
                    // Si hay al menos uno en POR_PROCESAR, cambiar a POR_PROCESAR
                    if (ckdLot.Status != LoteStatus.ToProcess)
                    {
                        ckdLot.Status = LoteStatus.ToProcess;
                        _dbContext.Set<Lot>().Update(ckdLot);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
