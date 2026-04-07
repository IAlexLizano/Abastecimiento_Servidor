using Auth.DTOs;
using Auth.Wrappers;

namespace Auth.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de login
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Realiza el inicio de sesión del usuario
        /// </summary>
        Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// Obtiene el menú para un usuario específico
        /// </summary>
        Task<Response<List<MenuItemResponseDto>>> GetMenuByUserAsync(int userId);
    }
}
