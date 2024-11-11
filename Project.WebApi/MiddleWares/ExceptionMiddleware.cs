using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Sonraki middleware'e devam et
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // Hata varsa yakala ve işle
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Hata mesajını loglayalım (isteğe bağlı)
        Debug.WriteLine($"Hata: {exception.Message}");

        // Yanıt olarak döneceğimiz hata detayları
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Sunucuda bir hata oluştu. Lütfen daha sonra tekrar deneyin."
        });

        return context.Response.WriteAsync(result);
    }

}
