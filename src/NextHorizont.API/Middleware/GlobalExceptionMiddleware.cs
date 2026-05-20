using System.Net;
using System.Text.Json;
using FluentValidation;
using NextHorizont.Domain.Exceptions;

namespace NextHorizont.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            DomainException domainEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse("Error de Negocio", domainEx.Message)
            ),

            ValidationException validationEx => (
                HttpStatusCode.UnprocessableEntity,
                new ErrorResponse(
                    "Error de Validación",
                    "Uno o más campos no cumplen las reglas de validación.",
                    validationEx.Errors.Select(e => new FieldError(e.PropertyName, e.ErrorMessage)).ToArray()
                )
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse("No Autorizado", "No tienes permisos para realizar esta acción.")
            ),

            KeyNotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                new ErrorResponse("No Encontrado", notFoundEx.Message)
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse("Error Interno", "Ocurrió un error inesperado. Contacta al administrador.")
            )
        };

        _logger.LogError(exception, "Excepción capturada: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public record ErrorResponse(string Title, string Detail, FieldError[]? Errors = null);
public record FieldError(string Field, string Message);
