using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    /// <summary>
    /// Query para obtener información básica (id y código) de lotes, contenedores, pallets y cajas
    /// que están en estado "EN_PROCESO"
    /// </summary>
    public class GetBasicInformationQuery : IRequest<Response<IEnumerable<BasicInformationResponseDto>>>
    {
    }

    /// <summary>
    /// Handler para procesar la query de información básica
    /// </summary>
    public class GetBasicInformationQueryHandler : IRequestHandler<GetBasicInformationQuery, Response<IEnumerable<BasicInformationResponseDto>>>
    {
        private readonly IOpeningRepository _repository;

        public GetBasicInformationQueryHandler(IOpeningRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<IEnumerable<BasicInformationResponseDto>>> Handle(GetBasicInformationQuery request, CancellationToken cancellationToken)
        {
                var result = await _repository.GetBasicInformationInProcessAsync();
                return new Response<IEnumerable<BasicInformationResponseDto>>(result, "Información básica obtenida exitosamente");
        }
    }
}
