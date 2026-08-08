using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Users
{
    /// <summary>
    /// Query para obtener un usuario por su ID
    /// </summary>
    public class GetUserByIdQuery : IRequest<Response<UserResponseDto>>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Handler para la query de obtener usuario por ID
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponseDto>>
    {
        private readonly IUsersRepository _usersService;

        public GetUserByIdQueryHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _usersService.GetUserByIdAsync(request.UserId);
            return new Response<UserResponseDto>(response);
        }
    }
}
