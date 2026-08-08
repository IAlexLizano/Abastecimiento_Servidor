using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Commands.Users
{
    /// <summary>
    /// Comando para reactivar un usuario
    /// </summary>
    public class ReactivateUserCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Handler para el comando de reactivar usuario
    /// </summary>
    public class ReactivateUserCommandHandler : IRequestHandler<ReactivateUserCommand, Response<string>>
    {
        private readonly IUsersRepository _usersService;

        public ReactivateUserCommandHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<string>> Handle(ReactivateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _usersService.ReactivateUserAsync(request.UserId);
            return new Response<string>(response, "Usuario reactivado correctamente");
        }
    }
}
