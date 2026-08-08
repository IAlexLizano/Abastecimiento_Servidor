using GeneralApplication.DTOs;
using GeneralApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace GeneralApplication.Features.Queries
{
    public class GetAllRolesQuery : IRequest<Response<IEnumerable<RoleDto>>>
    {
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Response<IEnumerable<RoleDto>>>
    {
        private readonly IRoleRepository _repository;

        public GetAllRolesQueryHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllRoles();
            return new Response<IEnumerable<RoleDto>>(data, "Roles obtenidos exitosamente.");
        }
    }
}
