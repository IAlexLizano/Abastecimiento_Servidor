using AuthApplication.DTOs.Users;
using AuthApplication.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Commands.Users
{
    /// <summary>
    /// Comando para crear un nuevo usuario
    /// </summary>
    public class CreateUserCommand : IRequest<Response<string>>
    {
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string IdCard { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
    }

    /// <summary>
    /// Handler para el comando de crear usuario
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersService;

        public CreateUserCommandHandler(IMapper mapper, IUsersRepository usersService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task<Response<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var createUserDto = _mapper.Map<CreateUserRequestDto>(request);
            var response = await _usersService.CreateUserAsync(createUserDto);
            return new Response<string>(response, "Usuario creado correctamente");
        }
    }
}
