namespace Shared
{
    public static class ErrorExtensions
    {
        public static object ErrorSolve(this Exception exception)
        {
            string message = string.Empty;
            object? errores = null;
            try
            {
                Exception? exceptionLocal = exception;

                while (exceptionLocal != null)
                {
                    message = exceptionLocal.Message;
                    exceptionLocal = exceptionLocal.InnerException;
                }


                errores = exception.GetPropertyValue("Errors");

            }
            catch (Exception)
            {

            }
            return new { Succeeded = false, Message = message, Errors = errores };
        }

    }
}
