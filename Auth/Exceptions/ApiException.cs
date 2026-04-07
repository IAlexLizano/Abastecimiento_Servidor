namespace Auth.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores de API
    /// </summary>
    public class ApiException : Exception
    {
        public ApiException(string? message = null) : base(message)
        {
        }

        public ApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
