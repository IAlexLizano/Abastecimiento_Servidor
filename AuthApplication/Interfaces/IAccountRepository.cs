using AuthApplication.DTOs;
using Shared.Application.Wrappers;

namespace AuthApplication.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de cuenta y generación de tokens JWT
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
    }
}
