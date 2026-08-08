using AuthApplication.Interfaces;
using AuthApplication.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries
{
    public class ObtainMenuQuery : IRequest<Response<List<MenuItemResponseDto>>>
    {
    }

    public class ObtainMenuQueryHandler : IRequestHandler<ObtainMenuQuery, Response<List<MenuItemResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginServices;
        private readonly IConfiguration _configuration;

        public ObtainMenuQueryHandler(IMapper mapper, ILoginRepository loginServices, IConfiguration configuration)
        {
            _mapper = mapper;
            _loginServices = loginServices;
            _configuration = configuration;
        }

        public async Task<Response<List<MenuItemResponseDto>>> Handle(ObtainMenuQuery request, CancellationToken cancellationToken)
        {
            List<MenuItemResponseDto> respuesta = await _loginServices.GetMenuByUserAsync();

            return new Response<List<MenuItemResponseDto>>(respuesta);
        }
    }
}
