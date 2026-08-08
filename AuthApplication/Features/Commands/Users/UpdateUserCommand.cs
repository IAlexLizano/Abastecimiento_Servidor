using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Commands.Users
{
    /// <summary>
    /// Comando para actualizar un usuario
    /// </summary>
    public class UpdateUserCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
    }

    /// <summary>
    /// Handler para el comando de actualizar usuario
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersService;

        public UpdateUserCommandHandler(IMapper mapper, IUsersRepository usersService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateUserDto = _mapper.Map<UpdateUserRequestDto>(request);
            var response = await _usersService.UpdateUserAsync(request.UserId, updateUserDto);
            return new Response<string>(response, "Usuario actualizado correctamente");
        }
    }
}
