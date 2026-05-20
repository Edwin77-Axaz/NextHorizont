using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using NextHorizont.Services;

namespace NextHorizont
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddAuthorizationCore();

            // Configurar HttpClient que ignora errores SSL en desarrollo local
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
            builder.Services.AddScoped(sp => new HttpClient(handler) { BaseAddress = new Uri("https://localhost:7025") });
            
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
