using AutoMapper.Execution;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using OpeningApplication.DTOs.Teams;
using OpeningApplication.Interfaces;
using OpeningPersistance.Context;
using Shared.Application.Exceptions;
using Shared.Global;

namespace OpeningPersistance.Repository
{
    /// <summary>
    /// Repository para gestionar operaciones de Apertura (Opening).
    /// 
    /// Esta clase implementa la lógica de datos para todas las operaciones relacionadas
    /// con la apertura y verificación de pallets en el sistema CiautoAbs. Maneja:
    /// - Asignación de equipos de inspección a pallets
    /// - Verificación de cajas y sus productos
    /// - Finalización de cajas (completado)
    /// - Marcado de componentes para ingeniería
    /// - Validación de cascada de estados
    /// </summary>
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly InformationSession _global;

        public TeamRepository(ApplicationContext dbContext, InformationSession global)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _global = global ?? throw new ArgumentNullException(nameof(global));
        }

        public async Task<string> AssignTeamToBoxAsync(AssignTeamToBoxRequestDto request)
        {
            try
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                BoxInspectionTeam team = new()
                {
                    IdBox = request.BoxId,
                    CreatedAt = DateTime.Now,
                };
                await _dbContext.BoxInspectionTeam.AddAsync(team);

                //if (!request.TeamMembers.Any(m => m.UserId == _global.UserId))
                //    throw new ApiException("El usuario actual no está incluido en el equipo asignado.");

                foreach (var member in request.TeamMembers)
                {
                    BoxInspectionTeamMember teamMember = new()
                    {
                        IdTeamNavigation = team,
                        IdUser = member.UserId,
                        AssignedColor = member.AssignedColor ?? string.Empty
                    };
                    await _dbContext.BoxInspectionTeamMember.AddAsync(teamMember);
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Equipo asignado exitosamente a la caja.";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al asignar el equipo a la caja: {ex.Message}");
            }
        }

        public async Task<TeamByBoxResponseDto> GetTeamAssignmentByBoxAsync(int boxId)
        {
            try
            {
                var team = await (from t in _dbContext.BoxInspectionTeam
                                  join b in _dbContext.Box on t.IdBox equals b.IdBox
                                  join m in _dbContext.BoxInspectionTeamMember on t.IdTeam equals m.IdTeam into teamMembers
                                  where t.IdBox == boxId
                                  select new TeamByBoxResponseDto
                                  {
                                      IdTeam = t.IdTeam,
                                      IdBox = t.IdBox ?? 0,
                                      BoxCode = b.BoxNumber,
                                      Members = teamMembers.Select(m => new TeamMemberDto
                                      {
                                          UserId = m.IdUser,
                                          AssignedColor = m.AssignedColor ?? string.Empty
                                      }).ToList()
                                  }).FirstOrDefaultAsync() ?? throw new ApiException("No existe equipo asignado para la caja seleccionada");
                return team;
            }
            catch (Exception)
            {
                throw new ApiException("Error al obtener equipo asignado para la caja seleccionada");
            }
        }
    }
}