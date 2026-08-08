namespace AuthApplication.Interfaces
{
    /// <summary>
    /// Interfaz para servicios de hash de contraseñas
    /// </summary>
    public interface IHashingRepository
    {
        /// <summary>
        /// Genera un hash de una contraseña
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifica si una contraseña coincide con su hash
        /// </summary>
        bool VerifyPassword(string password, string hash);
    }
}
