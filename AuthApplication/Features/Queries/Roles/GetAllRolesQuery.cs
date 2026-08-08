using AuthApplication.DTOs.Roles;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Roles
{
    /// <summary>
    /// Query para obtener todos los roles
    /// </summary>
    public class GetAllRolesQuery : IRequest<Response<List<RoleResponseDto>>>
    {
    }

    /// <summary>
    /// Handler para la query de obtener todos los roles
    /// </summary>
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Response<List<RoleResponseDto>>>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetAllRolesQueryHandler(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        public async Task<Response<List<RoleResponseDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var response = await _rolesRepository.GetAllRolesAsync();
            return new Response<List<RoleResponseDto>>(response, "Roles obtenidos correctamente");
        }
    }
}
