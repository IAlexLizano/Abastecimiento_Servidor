using AuthApplication.DTOs.Users;
using Shared.Application.Wrappers;

namespace AuthApplication.Interfaces
{
    /// <summary>
    /// Interfaz para la gestión de usuarios
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Crea un nuevo usuario. La contraseña se genera automáticamente con la cédula del usuario.
        /// </summary>
        Task<string> CreateUserAsync(CreateUserRequestDto request);

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        Task<UserResponseDto> GetUserByIdAsync(int userId);

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        Task<UserResponseDto> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Obtiene todos los usuarios activos
        /// </summary>
        Task<List<UserResponseDto>> GetAllUsersAsync();

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        Task<string> UpdateUserAsync(int userId, UpdateUserRequestDto request);

        /// <summary>
        /// Desactiva un usuario (cambio lógico)
        /// </summary>
        Task<string> DeactivateUserAsync(int userId);

        /// <summary>
        /// Reactiva un usuario
        /// </summary>
        Task<string> ReactivateUserAsync(int userId);

        /// <summary>
        /// Verifica si un nombre de usuario ya existe
        /// </summary>
        Task<bool> UserExistsByUsernameAsync(string username);

        /// <summary>
        /// Verifica si una cédula ya existe
        /// </summary>
        Task<bool> UserExistsByIdCardAsync(string idCard);

        /// <summary>
        /// Obtiene todos los usuarios que tengan un rol específico
        /// </summary>
        Task<List<UserResponseDto>> GetUsersByRoleIdAsync(int roleId);
    }
}
