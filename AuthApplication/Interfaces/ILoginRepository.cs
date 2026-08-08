using AuthApplication.DTOs;
using Shared.Application.Wrappers;

namespace AuthApplication.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de login
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Realiza el inicio de sesión del usuario
        /// </summary>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// Obtiene el menú para un usuario específico
        /// </summary>
        Task<List<MenuItemResponseDto>> GetMenuByUserAsync();
    }
}
