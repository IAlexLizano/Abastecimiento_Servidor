using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using NoveltyPersistance.Context;
using NoveltyApplication.Interfaces;
using NoveltyApplication.DTOs;
using Shared;
using Shared.Global;

namespace NoveltyPersistance.Repository
{
    /// <summary>
    /// Repository para gestionar operaciones de Novedades (Novelty)
    /// Maneja registro, seguimiento y resolución de fallos encontrados en componentes
    /// </summary>
    public class NoveltyRepository : INoveltyRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly InformationSession _global;

        public NoveltyRepository(ApplicationContext dbContext, InformationSession global)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        #region Novelty Registration

        /// <summary>
        /// Registra una novedad (fallo) encontrado en un componente durante apertura
        /// </summary>
        public async Task<EngineeringIssueDto> RegisterNoveltyAsync(RegisterNoveltyRequestDto request)
        {
            try
            {
                // Buscar detalle usando idCaja y idComponente
                var detail = await _dbContext.Set<BoxProductDetail>()
                    .Include(bd => bd.IdComponentNavigation)
                    .Include(bd => bd.IdBoxNavigation)
                    .FirstOrDefaultAsync(bd => bd.IdBox == request.BoxId && bd.IdComponent == request.ComponentId)
                    ?? throw new ApiException($"Detalle para Caja {request.BoxId} y Componente {request.ComponentId} no encontrado");

                // Crear el issue de ingeniería (novedad)
                var novelty = new EngineeringIssue
                {
                    IdDetail = detail.IdBoxProductDetail,
                    IdUserReports = _global.UserId,
                    Reason = request.Reason,
                    ReportDate = DateTime.Now
                };

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                // cantidad revisada el total de la caja - la cantidad del parametro
                detail.RevisedQuantity = detail.TotalQuantity - request.Quantity;
                _dbContext.Set<BoxProductDetail>().Update(detail);

                // modificar el estado de la caja a en revisión
                var box = detail.IdBoxNavigation;
                if (box != null)
                {
                    box.Status = LoteStatus.InReview;
                    _dbContext.Set<Box>().Update(box);
                }

                _dbContext.Set<EngineeringIssue>().Add(novelty);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                if (box != null)
                {
                    await UpdateCascadeUpFromBoxAsync(box.IdBox);
                }

                return MapEngineeringIssueToDto(novelty, detail, new RegisteredUser());
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene todos los issues de ingeniería (novedades) de una caja
        /// </summary>
        public async Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByBoxAsync(int boxId)
        {
            if (boxId <= 0)
                throw new ApiException("BoxId inválido");

            // Verificar que la caja existe
            var box = await _dbContext.Set<Box>()
                .FirstOrDefaultAsync(b => b.IdBox == boxId);

            if (box == null)
                throw new ApiException($"Box con ID {boxId} no encontrado");

            var novelties = await _dbContext.Set<EngineeringIssue>()
                .AsNoTracking()
                .Where(ei => ei.IdDetailNavigation.IdBox == boxId)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdBoxNavigation)
                .Include(ei => ei.IdUserReportsNavigation)
                .OrderByDescending(ei => ei.ReportDate)
                .ToListAsync();

            return novelties.Select(n => MapEngineeringIssueToDto(n, n.IdDetailNavigation, n.IdUserReportsNavigation));
        }

        /// <summary>
        /// Obtiene todos los issues de ingeniería (novedades) de un pallet
        /// </summary>
        public async Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByPalletAsync(int palletId)
        {
            if (palletId <= 0)
                throw new ApiException("PalletId inválido");

            // Verificar que el pallet existe
            var pallet = await _dbContext.Set<Pallet>()
                .FirstOrDefaultAsync(p => p.IdPallet == palletId);

            if (pallet == null)
                throw new ApiException($"Pallet con ID {palletId} no encontrado");

            var novelties = await _dbContext.Set<EngineeringIssue>()
                .AsNoTracking()
                .Where(ei => ei.IdDetailNavigation.IdBoxNavigation.IdPallet == palletId)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdBoxNavigation)
                .Include(ei => ei.IdUserReportsNavigation)
                .OrderByDescending(ei => ei.ReportDate)
                .ToListAsync();

            return novelties.Select(n => MapEngineeringIssueToDto(n, n.IdDetailNavigation, n.IdUserReportsNavigation));
        }

        /// <summary>
        /// Obtiene un issue específico de ingeniería (novedad)
        /// </summary>
        public async Task<EngineeringIssueDto> GetNoveltyByIdAsync(int issueId)
        {
            if (issueId <= 0)
                throw new ApiException("IssueId inválido");

            var novelty = await _dbContext.Set<EngineeringIssue>()
                .AsNoTracking()
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdBoxNavigation)
                .Include(ei => ei.IdUserReportsNavigation)
                .FirstOrDefaultAsync(ei => ei.IdIssue == issueId);

            if (novelty == null)
                throw new ApiException($"EngineeringIssue con ID {issueId} no encontrado");

            return MapEngineeringIssueToDto(novelty, novelty.IdDetailNavigation, novelty.IdUserReportsNavigation);
        }

        #endregion

        #region Novelty Status Operations

        /// <summary>
        /// Actualiza el estado de resolución de una novedad
        /// </summary>
        public async Task UpdateNoveltyStatusAsync(int issueId, string resolutionStatus)
        {
            if (issueId <= 0 || string.IsNullOrWhiteSpace(resolutionStatus))
                throw new ApiException("IssueId y ResolutionStatus son requeridos");

            // Validar que el estado es válido
            var validStatuses = new[] { "REPORTADA", "EN_REVISIÓN", "RESUELTA", "PENDIENTE" };
            if (!validStatuses.Contains(resolutionStatus))
                throw new ApiException($"Estado '{resolutionStatus}' no es válido");

            var novelty = await _dbContext.Set<EngineeringIssue>()
                .FirstOrDefaultAsync(ei => ei.IdIssue == issueId);

            if (novelty == null)
                throw new ApiException($"EngineeringIssue con ID {issueId} no encontrado");

            novelty.ResolutionStatus = resolutionStatus;
            _dbContext.Set<EngineeringIssue>().Update(novelty);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Resuelve una novedad (cambia estado a RESUELTA y actualiza fecha de resolución)
        /// </summary>
        public async Task<string> ResolveNoveltyAsync(int issueId)
        {
            var novelty = await _dbContext.Set<EngineeringIssue>()
                .FirstOrDefaultAsync(ei => ei.IdIssue == issueId);

            if (novelty == null)
                throw new ApiException($"EngineeringIssue con ID {issueId} no encontrado");

            novelty.ResolutionStatus = "RESUELTA";
            novelty.SolveDate = DateTime.Now;

            _dbContext.Set<EngineeringIssue>().Update(novelty);
            await _dbContext.SaveChangesAsync();

            // Re-evaluar el estado si ya no hay novedades
            var detail = await _dbContext.Set<BoxProductDetail>().FirstOrDefaultAsync(bd => bd.IdBoxProductDetail == novelty.IdDetail);
            if (detail != null)
            {
                bool hasOpenIssues = await _dbContext.Set<EngineeringIssue>()
                    .AnyAsync(ei => ei.IdDetailNavigation.IdBox == detail.IdBox && ei.ResolutionStatus != "RESUELTA");
                
                if (!hasOpenIssues)
                {
                    var box = await _dbContext.Set<Box>().FirstOrDefaultAsync(b => b.IdBox == detail.IdBox);
                    if (box != null && box.Status == LoteStatus.InReview)
                    {
                        box.Status = LoteStatus.InProcess;
                        _dbContext.Set<Box>().Update(box);
                        await _dbContext.SaveChangesAsync();
                        await UpdateCascadeUpFromBoxAsync(box.IdBox);
                    }
                }
            }

            return "Novedad resuelta exitosamente";
        }

        /// <summary>
        /// Acepta una novedad (cambia estado a EN_PROGRESO)
        /// </summary>
        public async Task<string> AcceptNoveltyAsync(int issueId)
        {
            var novelty = await _dbContext.Set<EngineeringIssue>()
                .FirstOrDefaultAsync(ei => ei.IdIssue == issueId);

            if (novelty == null)
                throw new ApiException($"EngineeringIssue con ID {issueId} no encontrado");

            novelty.ResolutionStatus = SupplyStatus.InProgress;
            novelty.IdUserSolves = _global.UserId;

            _dbContext.Set<EngineeringIssue>().Update(novelty);
            await _dbContext.SaveChangesAsync();

            return "Novedad aceptada exitosamente";
        }

        /// <summary>
        /// Obtiene todas las novedades abiertas (no resueltas)
        /// </summary>
        public async Task<IEnumerable<EngineeringIssueDto>> GetOpenNovelties()
        {
            var novelties = await _dbContext.Set<EngineeringIssue>()
                .AsNoTracking()
                .Where(ei => ei.ResolutionStatus != "RESUELTA")
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdBoxNavigation)
                .Include(ei => ei.IdUserReportsNavigation)
                .OrderByDescending(ei => ei.ReportDate)
                .ToListAsync();

            return novelties.Select(n => MapEngineeringIssueToDto(n, n.IdDetailNavigation, n.IdUserReportsNavigation));

        }

        /// <summary>
        /// Obtiene novedades reportadas por un usuario específico
        /// </summary>
        public async Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ApiException("UserId inválido");

            var novelties = await _dbContext.Set<EngineeringIssue>()
                .AsNoTracking()
                .Where(ei => ei.IdUserReports == userId)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdComponentNavigation)
                .Include(ei => ei.IdDetailNavigation)
                    .ThenInclude(bd => bd.IdBoxNavigation)
                .Include(ei => ei.IdUserReportsNavigation)
                .OrderByDescending(ei => ei.ReportDate)
                .ToListAsync();

            return novelties.Select(n => MapEngineeringIssueToDto(n, n.IdDetailNavigation, n.IdUserReportsNavigation));
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Mapea una entidad EngineeringIssue a su DTO de respuesta
        /// </summary>
        private EngineeringIssueDto MapEngineeringIssueToDto(EngineeringIssue issue, BoxProductDetail? detail, RegisteredUser? user)
        {
            return new EngineeringIssueDto
            {
                IssueId = issue.IdIssue,
                DetailId = issue.IdDetail,
                ComponentId = detail?.IdComponent,
                ComponentCode = detail?.IdComponentNavigation?.PartCode,
                ComponentDescription = detail?.IdComponentNavigation?.Description,
                BoxId = detail?.IdBox,
                BoxCode = detail?.IdBoxNavigation?.BoxNumber,
                TotalQuantity = detail?.TotalQuantity,
                RevisedQuantity = detail?.RevisedQuantity,
                ReportedByUserId = issue.IdUserReports,
                ReportedByUsername = user?.Username,
                ReportedByFullName = $"{user?.FirstName} {user?.LastName}".Trim(),
                Reason = issue.Reason,
                ResolutionStatus = issue.ResolutionStatus,
                ReportDate = issue.ReportDate
            };
        }

        /// <summary>
        /// Actualiza el estado en cascada hacia arriba desde una caja (Box)
        /// Alarma de En Revisión se burbujea si al menos un hijo lo tiene.
        /// </summary>
        private async Task UpdateCascadeUpFromBoxAsync(int idBox)
        {
            var box = await _dbContext.Set<Box>()
                .Include(b => b.IdPalletNavigation)
                    .ThenInclude(p => p.Box)
                .Include(b => b.IdPalletNavigation)
                    .ThenInclude(p => p.IdContainerNavigation)
                        .ThenInclude(c => c.Pallet)
                .Include(b => b.IdPalletNavigation)
                    .ThenInclude(p => p.IdContainerNavigation)
                        .ThenInclude(c => c.IdLotNavigation)
                            .ThenInclude(l => l.Container)
                .FirstOrDefaultAsync(b => b.IdBox == idBox);

            if (box == null || box.IdPalletNavigation == null)
                return;

            var pallet = box.IdPalletNavigation;
            
            // 1. Update Pallet
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
                else if (boxStatuses.Contains(LoteStatus.InReview))
                {
                    if (pallet.Status != LoteStatus.InReview)
                    {
                        pallet.Status = LoteStatus.InReview;
                        _dbContext.Set<Pallet>().Update(pallet);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();

            // 2. Update Container
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
                    else if (palletStatuses.Contains(LoteStatus.InReview))
                    {
                        if (container.Status != LoteStatus.InReview)
                        {
                            container.Status = LoteStatus.InReview;
                            _dbContext.Set<Container>().Update(container);
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();

                // 3. Update Lot
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
                        else if (containerStatuses.Contains(LoteStatus.InReview))
                        {
                            if (lot.Status != LoteStatus.InReview)
                            {
                                lot.Status = LoteStatus.InReview;
                                _dbContext.Set<Lot>().Update(lot);
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}
