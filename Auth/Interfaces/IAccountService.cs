using Auth.DTOs;
using Auth.Wrappers;

namespace Auth.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de cuenta y generación de tokens JWT
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
    }
}
