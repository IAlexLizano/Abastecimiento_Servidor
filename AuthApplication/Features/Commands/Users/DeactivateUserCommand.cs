using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Commands.Users
{
    /// <summary>
    /// Comando para desactivar un usuario
    /// </summary>
    public class DeactivateUserCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Handler para el comando de desactivar usuario
    /// </summary>
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Response<string>>
    {
        private readonly IUsersRepository _usersService;

        public DeactivateUserCommandHandler(IUsersRepository usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<string>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _usersService.DeactivateUserAsync(request.UserId);
            return new Response<string>(response, "Usuario desactivado correctamente");
        }
    }
}
