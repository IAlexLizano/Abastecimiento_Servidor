using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Application.Exceptions;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;
using SupplyingPersistance.Context;

namespace SupplyingPersistance.Repository
{
    /// <summary>
    /// Repository para gestionar operaciones de Abastecimiento (Supplying).
    /// Maneja transferencias de componentes a estaciones usando Cardboard.
    /// Todos los métodos usan DTOs, nunca retornan entidades directamente.
    /// </summary>
    public class SupplyingRepository : ISupplyingRepository
    {
        private readonly ApplicationContext _dbContext;

        public SupplyingRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Abastece una estación registrando el envío de cartones (Cardboard).
        /// Actualiza send_user, send_date y realiza cascada de estados.
        /// </summary>
        public async Task<SupplyDto> SupplyStationAsync(int idSupply, int idUserSend)
        {
            var supply = await _dbContext.Set<Supply>()
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdComponentNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdBoxNavigation)
                            .ThenInclude(b => b.IdPalletNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdRecipeNavigation)
                        .ThenInclude(r => r.IdStationNavigation)
                .FirstOrDefaultAsync(s => s.IdSupply == idSupply);

            if (supply == null)
                throw new ApiException("Supply no encontrado");

            supply.IdUserSend = idUserSend;
            supply.SendDate = DateTime.Now;
            supply.SupplyStatus = LoteStatus.PartiallySupplied;

            if (supply.IdCardboardNavigation != null)
            {
                supply.IdCardboardNavigation.Status = LoteStatus.PartiallySupplied;

                // Actualizar Supply a EN_PROGRESO
                supply.SupplyStatus = LoteStatus.InProcess;
            }

            _dbContext.Set<Supply>().Update(supply);
            await _dbContext.SaveChangesAsync();

            return MapSupplyToDto(supply);
        }

        /// <summary>
        /// Obtiene cartones pendientes de abastecimiento.
        /// Solo devuelve aquellos con estado PENDIENTE en Supply.
        /// </summary>
        public async Task<BoxListResponseDto> GetPendingBoxesAsync()
        {
            var supplies = await _dbContext.Set<Supply>()
                .Where(s => s.SupplyStatus == SupplyStatus.Pending)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdComponentNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdBoxNavigation)
                            .ThenInclude(b => b.IdPalletNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdRecipeNavigation)
                        .ThenInclude(r => r.IdStationNavigation)
                .Include(s => s.IdUserSendNavigation)
                .Include(s => s.IdUserReceiveNavigation)
                .ToListAsync();

            var boxes = new List<LabeledBoxDetailDto>();
            foreach (var supply in supplies)
            {
                if (supply.IdCardboardNavigation != null)
                {
                    boxes.Add(MapCardboardToDetailDto(supply.IdCardboardNavigation, supply));
                }
            }

            return new BoxListResponseDto
            {
                Boxes = boxes,
                TotalCount = boxes.Count
            };
        }

        /// <summary>
        /// Recibe un cartón registrando la recepción.
        /// Actualiza receive_user, receive_date y realiza cascada de estados.
        /// </summary>
        public async Task<SupplyDto> ReceiveBoxAsync(int idSupply, int idUserReceive)
        {
            var supply = await _dbContext.Set<Supply>()
                .Include(s => s.IdCardboardNavigation)
                .FirstOrDefaultAsync(s => s.IdSupply == idSupply);

            if (supply == null)
                throw new ApiException("Supply no encontrado");

            supply.IdUserReceive = idUserReceive;
            supply.ReceiveDate = DateTime.Now;
            supply.SupplyStatus = LoteStatus.Stocked;

            if (supply.IdCardboardNavigation != null)
            {
                supply.IdCardboardNavigation.Status = LoteStatus.Stocked;
            }

            _dbContext.Set<Supply>().Update(supply);
            await _dbContext.SaveChangesAsync();

            return MapSupplyToDto(supply);
        }

        /// <summary>
        /// Obtiene supplies en estado EN_PROCESO con todos sus detalles.
        /// </summary>
        public async Task<BoxListResponseDto> GetSuppliesInProgressAsync()
        {
            var supplies = await _dbContext.Set<Supply>()
                .Where(s => s.SupplyStatus == LoteStatus.InProcess)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdComponentNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdBoxNavigation)
                        .ThenInclude(bpd => bpd.IdBoxNavigation)
                            .ThenInclude(b => b.IdPalletNavigation)
                .Include(s => s.IdCardboardNavigation)
                    .ThenInclude(c => c.IdRecipeNavigation)
                        .ThenInclude(r => r.IdStationNavigation)
                .Include(s => s.IdUserSendNavigation)
                .Include(s => s.IdUserReceiveNavigation)
                .ToListAsync();

            var boxes = new List<LabeledBoxDetailDto>();
            foreach (var supply in supplies)
            {
                if (supply.IdCardboardNavigation != null)
                {
                    boxes.Add(MapCardboardToDetailDto(supply.IdCardboardNavigation, supply));
                }
            }

            return new BoxListResponseDto
            {
                Boxes = boxes,
                TotalCount = boxes.Count
            };
        }

        /// <summary>
        /// Obtiene todos los cartones con sus detalles, sin importar el estado.
        /// </summary>
        public async Task<BoxListResponseDto> GetAllBoxesAsync()
        {
            var cardboards = await _dbContext.Set<Cardboard>()
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdComponentNavigation)
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                .Include(c => c.IdRecipeNavigation)
                    .ThenInclude(r => r.IdStationNavigation)
                .Include(c => c.IdUserNavigation)
                .Include(c => c.Supply)
                    .ThenInclude(s => s.IdUserSendNavigation)
                .Include(c => c.Supply)
                    .ThenInclude(s => s.IdUserReceiveNavigation)
                .ToListAsync();

            var boxes = cardboards
                .Select(c => 
                {
                    var supply = c.Supply.FirstOrDefault();
                    return MapCardboardToDetailDto(c, supply);
                })
                .ToList();

            return new BoxListResponseDto
            {
                Boxes = boxes,
                TotalCount = boxes.Count
            };
        }

        /// <summary>
        /// Mapea una entidad Supply a SupplyDto.
        /// </summary>
        private SupplyDto MapSupplyToDto(Supply supply)
        {
            return new SupplyDto
            {
                IdSupply = supply.IdSupply,
                IdCardboard = supply.IdCardboard,
                IdUserSend = supply.IdUserSend,
                IdUserReceive = supply.IdUserReceive,
                SendDate = supply.SendDate,
                ReceiveDate = supply.ReceiveDate,
                SupplyStatus = supply.SupplyStatus
            };
        }

        /// <summary>
        /// Mapea una entidad Cardboard a LabeledBoxDetailDto.
        /// </summary>
        private LabeledBoxDetailDto MapCardboardToDetailDto(Cardboard cardboard, Supply? supply = null)
        {
            var productDetail = cardboard.IdBoxNavigation;
            var box = productDetail?.IdBoxNavigation;
            var pallet = box?.IdPalletNavigation;

            var productDetails = new List<BoxProductDetailResponseDto>();
            if (productDetail != null)
            {
                productDetails.Add(new BoxProductDetailResponseDto
                {
                    IdDetail = productDetail.IdBoxProductDetail,
                    IdBox = productDetail.IdBox,
                    IdComponent = productDetail.IdComponent,
                    ComponentDescription = productDetail.IdComponentNavigation?.Description ?? "N/A",
                    OrderedQty = productDetail.TotalQuantity,
                    VerifiedQty = productDetail.RevisedQuantity,
                    SupplyStatus = cardboard.Status
                });
            }

            var supplies = new List<SupplyDetailDto>();
            if (supply != null)
            {
                supplies.Add(new SupplyDetailDto
                {
                    IdSupply = supply.IdSupply,
                    IdCardboard = supply.IdCardboard,
                    IdUserSend = supply.IdUserSend,
                    IdUserReceive = supply.IdUserReceive,
                    SendDate = supply.SendDate,
                    ReceiveDate = supply.ReceiveDate,
                    SupplyStatus = supply.SupplyStatus,
                    SendUserName = supply.IdUserSendNavigation?.Username ?? "N/A",
                    ReceiveUserName = supply.IdUserReceiveNavigation?.Username ?? "N/A",
                    StationName = cardboard.IdRecipeNavigation?.IdStationNavigation?.Name ?? "N/A"
                });
            }

            return new LabeledBoxDetailDto
            {
                IdLabeledBox = cardboard.IdCardboard,
                IdBox = cardboard.IdBox,
                IdStation = cardboard.IdRecipeNavigation?.IdStation ?? 0,
                Quantity = cardboard.IdRecipeNavigation?.Total ?? 0, // Sacado del campo Total de la Receta
                SupplyStatus = cardboard.Status,
                SupplyDate = cardboard.OpeningDate,
                IdUser = cardboard.IdUser,
                UserName = cardboard.IdUserNavigation?.Username ?? "N/A",
                StationName = cardboard.IdRecipeNavigation?.IdStationNavigation?.Name ?? "N/A",
                ProductDetails = productDetails,
                PalletCode = pallet?.PalletNumber ?? "N/A",
                BoxCode = box?.BoxNumber ?? "N/A",
                Supplies = supplies
            };
        }

        /// <summary>
        /// Actualiza el estado en cascada hacia arriba desde un Cardboard
        /// Si todas las partes tienen el mismo estado, se actualiza el padre recursivamente
        /// </summary>
        private async Task UpdateCascadeUpFromCardboardAsync(int? idCardboard)
        {
            if (idCardboard == null) return;

            var cardboard = await _dbContext.Set<Cardboard>()
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                            .ThenInclude(p => p.IdContainerNavigation)
                                .ThenInclude(c => c.IdLotNavigation)
                                    .ThenInclude(l => l.Container)
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                            .ThenInclude(p => p.IdContainerNavigation)
                                .ThenInclude(c => c.Pallet)
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdBoxNavigation)
                        .ThenInclude(b => b.IdPalletNavigation)
                            .ThenInclude(p => p.Box)
                .Include(c => c.IdBoxNavigation)
                    .ThenInclude(bpd => bpd.IdBoxNavigation)
                        .ThenInclude(b => b.BoxProductDetail)
                            .ThenInclude(bpd => bpd.Cardboard)
                .FirstOrDefaultAsync(c => c.IdCardboard == idCardboard);

            if (cardboard == null || cardboard.IdBoxNavigation == null || cardboard.IdBoxNavigation.IdBoxNavigation == null)
                return;

            var box = cardboard.IdBoxNavigation.IdBoxNavigation;

            // 1. Update Box
            var allCardboardsInBox = box.BoxProductDetail.SelectMany(bpd => bpd.Cardboard).ToList();
            if (allCardboardsInBox.Any())
            {
                var cardStatuses = allCardboardsInBox.Select(c => c.Status).Distinct().ToList();
                if (cardStatuses.Count == 1)
                {
                    var newStatus = cardStatuses.First();
                    if (box.Status != newStatus)
                    {
                        box.Status = newStatus;
                        _dbContext.Set<Box>().Update(box);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();

            // 2. Update Pallet
            if (box.IdPalletNavigation != null)
            {
                var pallet = box.IdPalletNavigation;
                if (pallet.Box.Any())
                {
                    var boxStatuses = pallet.Box.Select(b => b.Status).Distinct().ToList();
                    if (boxStatuses.Count == 1)
                    {
                        var newStatus = boxStatuses.First();
                        if (pallet.Status != newStatus)
                        {
                            pallet.Status = newStatus;
                            _dbContext.Set<Pallet>().Update(pallet);
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();

                // 3. Update Container
                if (pallet.IdContainerNavigation != null)
                {
                    var container = pallet.IdContainerNavigation;
                    if (container.Pallet.Any())
                    {
                        var palletStatuses = container.Pallet.Select(p => p.Status).Distinct().ToList();
                        if (palletStatuses.Count == 1)
                        {
                            var newStatus = palletStatuses.First();
                            if (container.Status != newStatus)
                            {
                                container.Status = newStatus;
                                _dbContext.Set<Container>().Update(container);
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();

                    // 4. Update Lot
                    if (container.IdLotNavigation != null)
                    {
                        var lot = container.IdLotNavigation;
                        if (lot.Container.Any())
                        {
                            var containerStatuses = lot.Container.Select(c => c.Status).Distinct().ToList();
                            if (containerStatuses.Count == 1)
                            {
                                var newStatus = containerStatuses.First();
                                if (lot.Status != newStatus)
                                {
                                    lot.Status = newStatus;
                                    _dbContext.Set<Lot>().Update(lot);
                                }
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
