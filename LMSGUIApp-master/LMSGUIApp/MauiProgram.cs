using Microsoft.Extensions.Logging;

namespace LMSGUIApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            //register the following services -->
            builder.Services.AddSingleton<MainViewModel>();//add this
            builder.Services.AddTransient<DataPage>();//add this
            builder.Services.AddTransient<ReportPage>();//add this
            return builder.Build();
        }
    }
}
