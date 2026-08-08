using Microsoft.Extensions.Primitives;
using Shared.Application.Exceptions;
using Shared.Application.Wrappers;
using Shared.Global;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MS_NoveltyQuery.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private InformationSession _global;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, InformationSession global)
        {
            try
            {
                _global = global;
                await ObtenerDatosToken(context.Request);
                await _next(context);
            }
            catch (Exception error)
            {
                //var resultE = JsonSerializer.Serialize(error.Message.);
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error.Message };

                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync(); //Json de los parámetros de entrada para logs
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }

        private async Task ObtenerDatosToken(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues headerValues))
                await headerValues.ObtenerDatosTokenAsync(_global);
        }
    }
}
