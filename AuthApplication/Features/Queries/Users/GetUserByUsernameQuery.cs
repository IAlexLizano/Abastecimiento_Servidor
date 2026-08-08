using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Users
{
    /// <summary>
    /// Query para obtener un usuario por su nombre de usuario
    /// </summary>
    public class GetUserByUsernameQuery : IRequest<Response<UserResponseDto>>
    {
        public string Username { get; set; } = null!;
    }

    /// <summary>
    /// Handler para la query de obtener usuario por username
    /// </summary>
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, Response<UserResponseDto>>
    {
        private readonly IUsersRepository _usersService;

        public GetUserByUsernameQueryHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var response = await _usersService.GetUserByUsernameAsync(request.Username);
            return new Response<UserResponseDto>(response);
        }
    }
}
