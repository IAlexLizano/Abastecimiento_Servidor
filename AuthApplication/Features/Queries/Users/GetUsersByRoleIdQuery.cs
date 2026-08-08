using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Users
{
    /// <summary>
    /// Query para obtener usuarios por rol específico
    /// </summary>
    public class GetUsersByRoleIdQuery : IRequest<Response<List<UserResponseDto>>>
    {
        public int RoleId { get; set; }
    }

    /// <summary>
    /// Handler para la query de obtener usuarios por rol
    /// </summary>
    public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, Response<List<UserResponseDto>>>
    {
        private readonly IUsersRepository _usersService;

        public GetUsersByRoleIdQueryHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<List<UserResponseDto>>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _usersService.GetUsersByRoleIdAsync(request.RoleId);
            return new Response<List<UserResponseDto>>(response);
        }
    }
}
