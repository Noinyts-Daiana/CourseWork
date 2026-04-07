using System.Net;
using CourseWork.Exceptions; // Не забудь підключити свої помилки

namespace CourseWork.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        
        catch (NotFoundException ex)
        {
            logger.LogWarning("Ресурс не знайдено: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning("Проблема з авторизацією: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ArgumentException ex) 
        {
            logger.LogWarning("Погані дані в запиті: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            await context.Response.WriteAsJsonAsync(new { message = "Передано некоректні дані." });
        }
        catch (FormatException ex)
        {
            logger.LogWarning("Помилка формату даних: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            await context.Response.WriteAsJsonAsync(new { message = "Невірний формат даних." });
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex, "Сервер впав");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "Внутрішня помилка сервера." });
        }
    }
}