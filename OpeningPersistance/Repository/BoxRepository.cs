using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using OpeningPersistance.Context;
using OpeningApplication.Interfaces;
using OpeningApplication.DTOs.Opening;
using Shared;
using Shared.Global;

namespace OpeningPersistance.Repository
{
    /// <summary>
    /// Repository para gestionar operaciones de Apertura (Opening) de cajas.
    /// 
    /// Maneja:
    /// - Obtención de cajas por pallet y por usuario
    /// - Registro de desempaque de cartones (unboxing)
    /// - Registro de apertura de cajas con cantidades verificadas
    /// </summary>
    public class BoxRepository : IBoxRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly InformationSession _global;

        public BoxRepository(ApplicationContext dbContext, InformationSession informationSession)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _global = informationSession ?? throw new ArgumentNullException(nameof(informationSession));
        }

        #region Box Query Operations

        /// <summary>
        /// Obtiene todas las cajas de un pallet con sus detalles completos incluyendo cardboards
        /// </summary>
        public async Task<IEnumerable<BoxWithLabeledBoxesResponseDto>> GetBoxesByPalletAsync(int palletId)
        {
            if (palletId <= 0)
                throw new ApiException("Id del pallet inválido");

            var boxes = await _dbContext.Set<Box>()
                .AsNoTracking()
                .Where(b => b.IdPallet == palletId)
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.Cardboard)
                        .ThenInclude(c => c.IdRecipeNavigation)
                            .ThenInclude(r => r.IdStationNavigation)
                .Include(b => b.IdPalletNavigation)
                .OrderBy(b => b.BoxNumber)
                .ToListAsync();

            return boxes.Select(b => MapBoxToResponseDto(b)).ToList();
        }

        /// <summary>
        /// Obtiene la última caja asignada a un usuario (solo cajas en proceso)
        /// La caja se obtiene a través del equipo de inspección asignado
        /// </summary>
        public async Task<BoxWithLabeledBoxesResponseDto> GetBoxesByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ApiException("Id del usuario inválido");

            // Obtener la última caja asignada al usuario a través de BoxInspectionTeam
            // Se traen los datos a memoria antes de hacer Distinct y Select para evitar errores de traducción de EF Core
            var boxes = await _dbContext.Set<BoxInspectionTeamMember>()
                .AsNoTracking()
                .Where(btm => btm.IdUser == userId)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.BoxProductDetail)
                            .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.BoxProductDetail)
                            .ThenInclude(bd => bd.Cardboard)
                                .ThenInclude(c => c.IdRecipeNavigation)
                                    .ThenInclude(r => r.IdStationNavigation)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                .ToListAsync();

            var box = boxes
                .Select(btm => btm.IdTeamNavigation.IdBoxNavigation)
                .Where(b => b != null && b.Status == LoteStatus.InProcess)
                .DistinctBy(b => b.IdBox)
                .OrderByDescending(b => b.BoxNumber)
                .FirstOrDefault();

            if (box == null)
                throw new ApiException($"No se encontró ninguna caja asignada al usuario con ID {userId}");

            return MapBoxToResponseDto(box);
        }

        /// <summary>
        /// Obtiene todas las cajas asignadas a un usuario sin importar su estado
        /// Las cajas se obtienen a través del equipo de inspección asignado
        /// </summary>
        public async Task<IEnumerable<BoxWithLabeledBoxesResponseDto>> GetAllBoxesByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ApiException("Id del usuario inválido");

            // Obtener todas las cajas asignadas al usuario a través de BoxInspectionTeam
            // Se traen los datos a memoria antes de hacer Distinct y Select para evitar errores de traducción de EF Core
            var boxes = await _dbContext.Set<BoxInspectionTeamMember>()
                .AsNoTracking()
                .Where(btm => btm.IdUser == userId)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.BoxProductDetail)
                            .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.BoxProductDetail)
                            .ThenInclude(bd => bd.Cardboard)
                                .ThenInclude(c => c.IdRecipeNavigation)
                                    .ThenInclude(r => r.IdStationNavigation)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                .ToListAsync();

            return boxes
                .Select(btm => btm.IdTeamNavigation.IdBoxNavigation)
                .Where(b => b != null)
                .DistinctBy(b => b.IdBox)
                .OrderBy(b => b.BoxNumber)
                .Select(b => MapBoxToResponseDto(b))
                .ToList();
        }

        /// <summary>
        /// Obtiene una caja específica por ID con todos sus detalles incluyendo cardboards
        /// </summary>
        public async Task<BoxWithLabeledBoxesResponseDto> GetBoxAsync(int boxId)
        {
            if (boxId <= 0)
                throw new ApiException("Id de la caja inválido");

            var box = await _dbContext.Set<Box>()
                .AsNoTracking()
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.Cardboard)
                        .ThenInclude(c => c.IdRecipeNavigation)
                            .ThenInclude(r => r.IdStationNavigation)
                .Include(b => b.IdPalletNavigation)
                .FirstOrDefaultAsync(b => b.IdBox == boxId);

            if (box == null)
                throw new ApiException($"Caja con ID {boxId} no encontrada");

            return MapBoxToResponseDto(box);
        }

        /// <summary>
        /// Obtiene todas las cajas procesadas del día actual asignadas a un usuario
        /// Las cajas se filtran por usuario a través del equipo de inspección
        /// Solo retorna información básica sin detalles de cardboards
        /// </summary>
        public async Task<IEnumerable<ProcessedBoxTodayResponseDto>> GetProcessedBoxesTodayByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ApiException("Id del usuario inválido");

            var today = DateOnly.FromDateTime(DateTime.Now);

            // Obtener todas las cajas asignadas al usuario a través de BoxInspectionTeam
            // Se traen los datos a memoria para evitar errores de traducción de EF Core
            var boxes = await _dbContext.Set<BoxInspectionTeamMember>()
                .AsNoTracking()
                .Where(btm => btm.IdUser == userId)
                .Include(btm => btm.IdTeamNavigation)
                    .ThenInclude(t => t.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                .ToListAsync();

            // Filtrar en memoria: cajas procesadas de hoy, distintas
            var processedBoxesToday = boxes
                .Select(btm => new { Team = btm.IdTeamNavigation, Box = btm.IdTeamNavigation?.IdBoxNavigation })
                .Where(x => x.Box != null && 
                            x.Box.Status == LoteStatus.Verified && 
                            x.Team?.CreatedAt.HasValue == true &&
                            DateOnly.FromDateTime(x.Team.CreatedAt.Value) == today)
                .DistinctBy(x => x.Box.IdBox)
                .OrderByDescending(x => x.Box.BoxNumber)
                .Select(x => new ProcessedBoxTodayResponseDto
                {
                    IdBox = x.Box.IdBox,
                    BoxCode = x.Box.BoxNumber,
                    Status = x.Box.Status,
                    IdPallet = x.Box.IdPallet,
                    PalletCode = x.Box.IdPalletNavigation?.PalletNumber,
                    CreatedAt = x.Team!.CreatedAt
                })
                .ToList();

            return processedBoxesToday;
        }

        #endregion

        #region Box Operation Commands

        /// <summary>
        /// Registra el desempaque de un cardboard específico
        /// Actualiza solo ese cardboard asignando el usuario activo y cambiando estado a PROCESADO
        /// No realiza actualización en cascada - cada cardboard se actualiza independientemente
        /// </summary>
        public async Task<UnboxingRegisteredResponseDto> RegisterUnboxingAsync(RegisterUnboxingRequestDto request)
        {
            if (request == null || request.IdLabeledBox <= 0)
                throw new ApiException("ID del cardboard inválido");

            // Obtener el cardboard específico
            var cardboard = await _dbContext.Set<Cardboard>()
                .Include(c => c.IdRecipeNavigation)
                    .ThenInclude(r => r.IdStationNavigation)
                .Include(c => c.IdBoxNavigation)
                .FirstOrDefaultAsync(c => c.IdCardboard == request.IdLabeledBox);

            if (cardboard == null)
                throw new ApiException($"Cardboard con ID {request.IdLabeledBox} no encontrada");

            // Actualizar cardboard con usuario activo y estado PROCESADO
            cardboard.IdUser = _global.UserId;
            cardboard.Status = LoteStatus.Verified;
            cardboard.OpeningDate = DateTime.Now;

            _dbContext.Set<Cardboard>().Update(cardboard);
            await _dbContext.SaveChangesAsync();

            // Retornar respuesta con el cardboard actualizado
            var updatedCardboard = new LabeledBoxDetailResponseDto
            {
                IdLabeledBox = cardboard.IdCardboard,
                IdStation = cardboard.IdRecipeNavigation?.IdStation ?? 0,
                StationName = cardboard.IdRecipeNavigation?.IdStationNavigation?.Name,
                Quantity = cardboard.IdRecipeNavigation?.Total ?? 1, // Obtener Total de la receta, por defecto 1
                SupplyStatus = cardboard.Status,
                SupplyDate = cardboard.OpeningDate,
                IdUser = cardboard.IdUser,
                UserFirstName = _global.FirstName,
                UserLastName = _global.UserName // O el campo que tenga el apellido
            };

            return new UnboxingRegisteredResponseDto
            {
                IdBox = cardboard.IdBox,
                BoxCode = cardboard.IdBoxNavigation?.IdBoxNavigation?.BoxNumber ?? string.Empty,
                Status = LoteStatus.InProcess,
                UpdatedLabeledBoxes = new List<LabeledBoxDetailResponseDto> { updatedCardboard }
            };
        }

        /// <summary>
        /// Registra la apertura de caja y realiza cascada de verificación.
        /// Solo requiere el ID de la caja.
        /// 
        /// Proceso:
        /// 1. Busca el primer box_product_detail
        /// 2. Asigna cantidad verificada = cantidad esperada
        /// 3. Marca caja como VERIFICADA
        /// 4. Cascada UP: Si todos los boxes del pallet están VERIFICADOS -> pallet VERIFICADO
        /// 5. Si todos los pallets del contenedor están VERIFICADOS -> contenedor VERIFICADO
        /// 6. Si todos los contenedores del lote están VERIFICADOS -> lote VERIFICADO
        /// </summary>
        public async Task<BoxOpeningRegisteredResponseDto> RegisterBoxOpeningAsync(RegisterBoxOpeningRequestDto request)
        {
            if (request == null || request.IdBox <= 0)
                throw new ApiException("ID de caja inválido");

            var box = await _dbContext.Set<Box>()
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(b => b.BoxProductDetail)
                    .ThenInclude(bd => bd.Cardboard)
                .Include(b => b.IdPalletNavigation)
                    .ThenInclude(p => p.IdContainerNavigation)
                        .ThenInclude(c => c.IdLotNavigation)
                .FirstOrDefaultAsync(b => b.IdBox == request.IdBox);

            if (box == null)
                throw new ApiException($"Caja con ID {request.IdBox} no encontrada");

            // Actualizar el primer box_product_detail: cantidad verificada = cantidad esperada
            if (box.BoxProductDetail != null && box.BoxProductDetail.Any())
            {
                var firstProductDetail = box.BoxProductDetail.First();
                firstProductDetail.RevisedQuantity = firstProductDetail.TotalQuantity;
                firstProductDetail.UpdateDate = DateTime.Now;
                _dbContext.Set<BoxProductDetail>().Update(firstProductDetail);
            }

            // Marcar caja como VERIFICADA
            box.Status = LoteStatus.Verified;
            _dbContext.Set<Box>().Update(box);
            await _dbContext.SaveChangesAsync();

            // Crear registros en tabla supply para cada cardboard de la caja
            if (box.BoxProductDetail != null && box.BoxProductDetail.Any())
            {
                foreach (var productDetail in box.BoxProductDetail)
                {
                    if (productDetail.Cardboard != null && productDetail.Cardboard.Any())
                    {
                        foreach (var cardboard in productDetail.Cardboard)
                        {
                            var newSupply = new Supply
                            {
                                IdCardboard = cardboard.IdCardboard,
                                SupplyStatus = SupplyStatus.Pending
                            };
                            _dbContext.Set<Supply>().Add(newSupply);
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

            // Realizar cascada de verificación
            if (box.IdPalletNavigation != null)
            {
                await UpdateCascadeVerificationAsync(box.IdPalletNavigation.IdContainerNavigation.IdLot);
            }

            // Retornar respuesta
            var updatedProductDetails = box.BoxProductDetail.Select(bd => new BoxProductDetailResponseDto
            {
                IdBoxProductDetail = bd.IdBoxProductDetail,
                IdComponent = bd.IdComponent,
                ComponentCode = bd.IdComponentNavigation?.PartCode,
                ComponentDescription = bd.IdComponentNavigation?.Description,
                ComponentName = bd.IdComponentNavigation?.Description,
                ExpectedQuantity = bd.TotalQuantity,
                RevisedQuantity = bd.RevisedQuantity,
                VerifiedQuantity = bd.RevisedQuantity,
                UpdateDate = bd.UpdateDate
            }).ToList();

            return new BoxOpeningRegisteredResponseDto
            {
                IdBox = box.IdBox,
                BoxCode = box.BoxNumber,
                Status = box.Status,
                ComponentsProcessed = updatedProductDetails.Count,
                UpdatedProductDetails = updatedProductDetails
            };
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Mapea entidad Box a DTO con cardboards
        /// </summary>
        private BoxWithLabeledBoxesResponseDto MapBoxToResponseDto(Box box)
        {
            var cardboards = new List<LabeledBoxDetailResponseDto>();

            if (box.BoxProductDetail != null)
            {
                foreach (var productDetail in box.BoxProductDetail)
                {
                    if (productDetail.Cardboard != null && productDetail.Cardboard.Any())
                    {
                        foreach (var cardboard in productDetail.Cardboard)
                        {
                            cardboards.Add(new LabeledBoxDetailResponseDto
                            {
                                IdLabeledBox = cardboard.IdCardboard,
                                IdStation = cardboard.IdRecipeNavigation?.IdStation ?? 0,
                                StationName = cardboard.IdRecipeNavigation?.IdStationNavigation?.Name,
                                Quantity = cardboard.IdRecipeNavigation?.Total ?? 1, // Obtener Total de la receta, por defecto 1
                                SupplyStatus = cardboard.Status,
                                SupplyDate = cardboard.OpeningDate,
                                IdUser = cardboard.IdUser,
                                UserFirstName = cardboard.IdUserNavigation?.FirstName,
                                UserLastName = cardboard.IdUserNavigation?.LastName
                            });
                        }
                    }
                }
            }

            var productDetails = box.BoxProductDetail?.Select(bd => new BoxProductDetailResponseDto
            {
                IdBoxProductDetail = bd.IdBoxProductDetail,
                IdComponent = bd.IdComponent,
                ComponentCode = bd.IdComponentNavigation?.PartCode,
                ComponentDescription = bd.IdComponentNavigation?.Description,
                ComponentName = bd.IdComponentNavigation?.Description,
                ExpectedQuantity = bd.TotalQuantity,
                RevisedQuantity = bd.RevisedQuantity,
                VerifiedQuantity = bd.RevisedQuantity,
                UpdateDate = bd.UpdateDate
            }).ToList() ?? new List<BoxProductDetailResponseDto>();

            return new BoxWithLabeledBoxesResponseDto
            {
                IdBox = box.IdBox,
                BoxCode = box.BoxNumber,
                Status = box.Status,
                IdPallet = box.IdPallet,
                PalletCode = box.IdPalletNavigation?.PalletNumber,
                ProductDetails = productDetails,
                LabeledBoxes = cardboards
            };
        }

        /// <summary>
        /// Actualiza el estado en cascada HACIA ARRIBA (UP) basado en verificación condicional
        /// Solo actualiza a VERIFICADO si TODOS los elementos del nivel anterior están VERIFICADOS
        /// 
        /// Lógica:
        /// 1. Si TODOS los boxes del pallet están VERIFICADOS -> pallet VERIFICADO
        /// 2. Si TODOS los pallets del contenedor están VERIFICADOS -> contenedor VERIFICADO
        /// 3. Si TODOS los contenedores del lote están VERIFICADOS -> lote VERIFICADO
        /// </summary>
        private async Task UpdateCascadeVerificationAsync(int lotId)
        {
            var lot = await _dbContext.Set<Lot>()
                .Include(c => c.Container)
                    .ThenInclude(con => con.Pallet)
                        .ThenInclude(p => p.Box)
                .FirstOrDefaultAsync(c => c.IdLot == lotId);

            if (lot == null)
                return;

            // Paso 1: Actualizar pallets basado en sus boxes
            foreach (var container in lot.Container)
            {
                foreach (var pallet in container.Pallet)
                {
                    var allBoxes = pallet.Box.ToList();
                    if (allBoxes.Any())
                    {
                        // Si TODOS los boxes están VERIFICADOS, entonces el pallet se marca VERIFICADO
                        var allBoxesVerified = allBoxes.All(b => b.Status == LoteStatus.Verified);
                        if (allBoxesVerified && pallet.Status != LoteStatus.Verified)
                        {
                            pallet.Status = LoteStatus.Verified;
                            _dbContext.Set<Pallet>().Update(pallet);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Paso 2: Actualizar contenedores basado en sus pallets
                var allPallets = container.Pallet.ToList();
                if (allPallets.Any())
                {
                    // Si TODOS los pallets están VERIFICADOS, entonces el contenedor se marca VERIFICADO
                    var allPalletsVerified = allPallets.All(p => p.Status == LoteStatus.Verified);
                    if (allPalletsVerified && container.Status != LoteStatus.Verified)
                    {
                        container.Status = LoteStatus.Verified;
                        _dbContext.Set<Container>().Update(container);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            // Paso 3: Actualizar lote basado en sus contenedores
            var allContainers = lot.Container.ToList();
            if (allContainers.Any())
            {
                // Si TODOS los contenedores están VERIFICADOS, entonces el lote se marca VERIFICADO
                var allContainersVerified = allContainers.All(c => c.Status == LoteStatus.Verified);
                if (allContainersVerified && lot.Status != LoteStatus.Verified)
                {
                    lot.Status = LoteStatus.Verified;
                    _dbContext.Set<Lot>().Update(lot);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Actualiza el estado en cascada desde un lote hacia abajo
        /// Solo cambia estado si todos los hijos tienen el nuevo estado
        /// </summary>
        private async Task UpdateCascadeStatusAsync(int lotId)
        {
            var lot = await _dbContext.Set<Lot>()
                .Include(c => c.Container)
                    .ThenInclude(con => con.Pallet)
                        .ThenInclude(p => p.Box)
                .FirstOrDefaultAsync(c => c.IdLot == lotId);

            if (lot == null)
                return;

            // Actualizar cajas a PROCESADO si todos los cardboards están en ese estado
            foreach (var container in lot.Container)
            {
                foreach (var pallet in container.Pallet)
                {
                    foreach (var box in pallet.Box)
                    {
                        var cardboards = await _dbContext.Set<Cardboard>()
                            .Where(c => c.IdBoxNavigation.IdBox == box.IdBox)
                            .ToListAsync();

                        if (cardboards.Any())
                        {
                            var allProcessed = cardboards.All(c => c.Status == LoteStatus.InProcess);
                            if (allProcessed && box.Status != LoteStatus.InProcess)
                            {
                                box.Status = LoteStatus.InProcess;
                                _dbContext.Set<Box>().Update(box);
                            }
                        }
                    }

                    // Actualizar pallets basado en cajas
                    var allBoxes = pallet.Box.ToList();
                    if (allBoxes.Any())
                    {
                        var allBoxesProcessed = allBoxes.All(b => b.Status == LoteStatus.InProcess);
                        var anyBoxProcessed = allBoxes.Any(b => b.Status == LoteStatus.InProcess);

                        if (allBoxesProcessed && pallet.Status != LoteStatus.InProcess)
                        {
                            pallet.Status = LoteStatus.InProcess;
                            _dbContext.Set<Pallet>().Update(pallet);
                        }
                        else if (anyBoxProcessed && pallet.Status != LoteStatus.InProcess)
                        {
                            pallet.Status = LoteStatus.InProcess;
                            _dbContext.Set<Pallet>().Update(pallet);
                        }
                    }
                }

                // Actualizar contenedores basado en pallets
                var allPallets = container.Pallet.ToList();
                if (allPallets.Any())
                {
                    var allPalletsProcessed = allPallets.All(p => p.Status == LoteStatus.InProcess);
                    var anyPalletProcessed = allPallets.Any(p => p.Status == LoteStatus.InProcess);

                    if (allPalletsProcessed && container.Status != LoteStatus.InProcess)
                    {
                        container.Status = LoteStatus.InProcess;
                        _dbContext.Set<Container>().Update(container);
                    }
                    else if (anyPalletProcessed && container.Status != LoteStatus.InProcess)
                    {
                        container.Status = LoteStatus.InProcess;
                        _dbContext.Set<Container>().Update(container);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            // Actualizar lote basado en contenedores
            var allContainers = lot.Container.ToList();
            if (allContainers.Any())
            {
                var allContainersProcessed = allContainers.All(c => c.Status == LoteStatus.InProcess);
                var anyContainerProcessed = allContainers.Any(c => c.Status == LoteStatus.InProcess);

                if (allContainersProcessed && lot.Status != LoteStatus.InProcess)
                {
                    lot.Status = LoteStatus.InProcess;
                    _dbContext.Set<Lot>().Update(lot);
                }
                else if (anyContainerProcessed && lot.Status != LoteStatus.InProcess)
                {
                    lot.Status = LoteStatus.InProcess;
                    _dbContext.Set<Lot>().Update(lot);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
