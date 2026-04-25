using Auth.Interfaces;
using AuthApplication.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Commands
{
    public class LoginCommand : IRequest<Response<LoginResponseDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<LoginResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILoginService _loginService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IMapper mapper, ILoginService loginService, IConfiguration configuration)
        {
            _mapper = mapper;
            _loginService = loginService;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginRequestDto login = _mapper.Map<LoginRequestDto>(request);
            Response<LoginResponseDto> respuesta = await _loginService.LoginAsync(login);

            return respuesta;
        }
    }
}
