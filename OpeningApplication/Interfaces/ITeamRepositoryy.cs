using OpeningApplication.DTOs.Teams;

namespace OpeningApplication.Interfaces
{
    public interface ITeamRepository
    {
        /// <summary>Asigna un equipo de inspección a una caja</summary>
        Task<string> AssignTeamToBoxAsync(AssignTeamToBoxRequestDto request);

        /// <summary>Obtiene el equipo asignado a una caja específica</summary>
        Task<TeamByBoxResponseDto> GetTeamAssignmentByBoxAsync(int boxId);
    }
}
