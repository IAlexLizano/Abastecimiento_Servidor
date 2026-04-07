namespace Auth.Exceptions
{
    /// <summary>
    /// Excepción para errores de validación
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string? message = null) : base(message)
        {
        }

        public ValidationException(Dictionary<string, string[]> failures)
            : base("Se han producido uno o más errores de validación")
        {
            Failures = failures;
        }

        public Dictionary<string, string[]> Failures { get; } = new();
    }
}
