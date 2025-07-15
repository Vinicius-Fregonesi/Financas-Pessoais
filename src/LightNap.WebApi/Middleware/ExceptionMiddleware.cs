using LightNap.Core.Api;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LightNap.WebApi.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions globally.
    /// </summary>
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions =
            new()
            {
                Converters = { new JsonStringEnumConverter() },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        /// <summary>
        /// Handles the HTTP request and catches exceptions.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
            catch (UserFriendlyApiException ex)
            {
                // Log error if inner exception exists
                if (ex.InnerException is not null)
                {
                    logger.LogError(ex, "User-friendly exception in Web API: {message}", ex.Message);
                }

                context.Response.Clear();

                // Aqui definimos 404 Not Found para UserFriendlyApiException
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var response = new ApiResponseDto<string>()
                {
                    ErrorMessages = ex.Errors ?? new[] { ex.Message },
                    Type = ApiResponseType.Error
                };

                var json = JsonSerializer.Serialize(response, _jsonSerializerOptions);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in Web API: {message}", ex.Message);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                ApiResponseDto<string> error;

                if (environment.IsDevelopment())
                {
                    if (string.IsNullOrWhiteSpace(ex.StackTrace))
                    {
                        error = new ApiResponseDto<string>()
                        {
                            Type = ApiResponseType.UnexpectedError,
                            ErrorMessages = new[] { ex.Message }
                        };
                    }
                    else
                    {
                        error = new ApiResponseDto<string>()
                        {
                            Type = ApiResponseType.UnexpectedError,
                            ErrorMessages = new[] { ex.Message, ex.StackTrace }
                        };
                    }
                }
                else
                {
                    error = new ApiResponseDto<string>()
                    {
                        Type = ApiResponseType.UnexpectedError,
                        ErrorMessages = new[] { "Internal Server Error" }
                    };
                }

                var json = JsonSerializer.Serialize(error, _jsonSerializerOptions);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
