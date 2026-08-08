using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Users
{
    /// <summary>
    /// Query para obtener todos los usuarios activos
    /// </summary>
    public class GetAllUsersQuery : IRequest<Response<List<UserResponseDto>>>
    {
    }

    /// <summary>
    /// Handler para la query de obtener todos los usuarios activos
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response<List<UserResponseDto>>>
    {
        private readonly IUsersRepository _usersService;

        public GetAllUsersQueryHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<List<UserResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var response = await _usersService.GetAllUsersAsync();
            return new Response<List<UserResponseDto>>(response);
        }
    }
}
