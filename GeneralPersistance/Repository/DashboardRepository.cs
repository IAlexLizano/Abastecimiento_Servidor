using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;
using GeneralApplication.Interfaces;
using GeneralPersistance.Context;
using GeneralApplication.DTOs.Dashboard;
using Shared;

namespace GeneralPersistance.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationContext _dbContext;

        public DashboardRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<DashboardKpisResponseDto> GetDashboardKpisAsync(DashboardFilterDto filter)
        {
            var response = new DashboardKpisResponseDto();

            // 1. Estado de contenedores
            var containerQuery = _dbContext.Container.AsQueryable();
            if (filter.IdLot.HasValue)
                containerQuery = containerQuery.Where(c => c.IdLot == filter.IdLot.Value);
            if (filter.StartDate.HasValue)
                containerQuery = containerQuery.Where(c => c.DisembarkationDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                containerQuery = containerQuery.Where(c => c.DisembarkationDate <= filter.EndDate.Value);

            var containerStatuses = await containerQuery
                .GroupBy(c => c.Status)
                .Select(g => new KpiStatusDto { Status = g.Key ?? "Desconocido", Count = g.Count() })
                .ToListAsync();

            response.ContainerStatus.AddRange(containerStatuses);

            // 2. Proceso de apertura de cajas
            // Joining Cardboard -> Box -> Pallet -> Container for Lot filtering
            var cardboardQuery = _dbContext.Cardboard
                .Include(c => c.IdBoxNavigation)
                .ThenInclude(bpd => bpd.IdBoxNavigation)
                .ThenInclude(b => b.IdPalletNavigation)
                .ThenInclude(p => p.IdContainerNavigation)
                .AsQueryable();

            if (filter.IdLot.HasValue)
                cardboardQuery = cardboardQuery.Where(c => c.IdBoxNavigation.IdBoxNavigation.IdPalletNavigation != null && c.IdBoxNavigation.IdBoxNavigation.IdPalletNavigation.IdContainerNavigation != null && c.IdBoxNavigation.IdBoxNavigation.IdPalletNavigation.IdContainerNavigation.IdLot == filter.IdLot.Value);
            if (filter.StartDate.HasValue)
                cardboardQuery = cardboardQuery.Where(c => c.OpeningDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                cardboardQuery = cardboardQuery.Where(c => c.OpeningDate <= filter.EndDate.Value);

            var cardboardStatuses = await cardboardQuery
                .GroupBy(c => c.Status)
                .Select(g => new KpiStatusDto { Status = g.Key ?? "Desconocido", Count = g.Count() })
                .ToListAsync();

            response.BoxOpening.AddRange(cardboardStatuses);

            // 3. Progreso de abastecimiento por estación
            // Using Recipe to identify station
            var supplyQuery = _dbContext.Cardboard
                .Include(c => c.IdRecipeNavigation)
                .ThenInclude(r => r.IdStationNavigation)
                .AsQueryable();

            if (filter.StartDate.HasValue)
                supplyQuery = supplyQuery.Where(c => c.OpeningDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                supplyQuery = supplyQuery.Where(c => c.OpeningDate <= filter.EndDate.Value);

            var supplyProgress = await supplyQuery
                .Where(c => c.IdRecipeNavigation != null && c.IdRecipeNavigation.IdStationNavigation != null)
                .GroupBy(c => new { Station = c.IdRecipeNavigation.IdStationNavigation.Name, Status = c.Status })
                .Select(g => new { g.Key.Station, g.Key.Status, Count = g.Count() })
                .ToListAsync();

            var stations = supplyProgress.Select(x => x.Station).Distinct();
            foreach (var station in stations)
            {
                var processed = supplyProgress.Where(x => x.Station == station && x.Status != null && (x.Status.ToUpper().Contains(LoteStatus.Verified) || x.Status.ToUpper().Contains(SupplyStatus.Completed) || x.Status.ToUpper().Contains("PROCESADO"))).Sum(x => x.Count);
                var pending = supplyProgress.Where(x => x.Station == station && x.Status != null && (x.Status.ToUpper().Contains(SupplyStatus.Pending) || x.Status.ToUpper().Contains(LoteStatus.ToProcess) || x.Status.ToUpper().Contains("PENDIENTE"))).Sum(x => x.Count);
                var supplied = supplyProgress.Where(x => x.Station == station && x.Status != null && (x.Status.ToUpper().Contains(LoteStatus.Stocked) || x.Status.ToUpper().Contains("ABASTECIDO"))).Sum(x => x.Count);
                // Also default non-processed to pending if no explicit 'pending' is used
                if (pending == 0 && supplied == 0)
                {
                    pending = supplyProgress.Where(x => x.Station == station && x.Status != null && !(x.Status.ToUpper().Contains(LoteStatus.Verified) || x.Status.ToUpper().Contains(SupplyStatus.Completed) || x.Status.ToUpper().Contains("PROCESADO"))).Sum(x => x.Count);
                }

                response.SupplyProgress.Add(new SupplyProgressKpiDto
                {
                    StationName = station ?? "Unknown",
                    Processed = processed,
                    Pending = pending,
                    Supplied = supplied
                });
            }

            // 4. Carga de inspección por operario
            var inspectionQuery = _dbContext.BoxInspectionTeamMember
                .Include(b => b.IdUserNavigation)
                .Include(b => b.IdTeamNavigation)
                .AsQueryable();

            if (filter.StartDate.HasValue)
                inspectionQuery = inspectionQuery.Where(b => b.IdTeamNavigation.CreatedAt >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                inspectionQuery = inspectionQuery.Where(b => b.IdTeamNavigation.CreatedAt <= filter.EndDate.Value);

            var inspections = await inspectionQuery
                .GroupBy(b => new { b.IdUserNavigation.FirstName, b.IdUserNavigation.LastName })
                .Select(g => new { Name = g.Key.FirstName + " " + g.Key.LastName, Count = g.Count() })
                .ToListAsync();

            var totalInspections = inspections.Sum(i => i.Count);
            foreach (var inspection in inspections)
            {
                response.InspectionLoad.Add(new InspectionLoadKpiDto
                {
                    OperatorName = inspection.Name,
                    ManagedBoxes = inspection.Count,
                    Percentage = totalInspections > 0 ? Math.Round((double)inspection.Count / totalInspections * 100, 2) : 0
                });
            }

            // 5. Seguimiento de incidencias
            var incidentQuery = _dbContext.EngineeringIssue.AsQueryable();

            if (filter.StartDate.HasValue)
                incidentQuery = incidentQuery.Where(e => e.ReportDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                incidentQuery = incidentQuery.Where(e => e.ReportDate <= filter.EndDate.Value);

            var incidents = await incidentQuery
                .GroupBy(e => e.ResolutionStatus)
                .Select(g => new KpiStatusDto { Status = g.Key ?? "Desconocido", Count = g.Count() })
                .ToListAsync();

            response.IncidentTracking.AddRange(incidents);

            return response;
        }
    }
}
