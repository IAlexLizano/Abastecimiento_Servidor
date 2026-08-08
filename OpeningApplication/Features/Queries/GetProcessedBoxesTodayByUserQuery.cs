using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    /// <summary>
    /// Query para obtener todas las cajas procesadas del día actual asignadas a un usuario
    /// </summary>
    public class GetProcessedBoxesTodayByUserQuery : IRequest<Response<IEnumerable<ProcessedBoxTodayResponseDto>>>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Handler para la query de obtener cajas procesadas de hoy por usuario
    /// </summary>
    public class GetProcessedBoxesTodayByUserQueryHandler : IRequestHandler<GetProcessedBoxesTodayByUserQuery, Response<IEnumerable<ProcessedBoxTodayResponseDto>>>
    {
        private readonly IBoxRepository _repository;

        public GetProcessedBoxesTodayByUserQueryHandler(IBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<ProcessedBoxTodayResponseDto>>> Handle(GetProcessedBoxesTodayByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetProcessedBoxesTodayByUserAsync(request.UserId);
                return new Response<IEnumerable<ProcessedBoxTodayResponseDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ProcessedBoxTodayResponseDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
