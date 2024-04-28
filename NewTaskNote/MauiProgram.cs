using Microsoft.Extensions.Logging;
using Acr.UserDialogs;
using Microsoft.Maui.LifecycleEvents;

namespace NewTaskNote
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
                })

                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                events.AddAndroid(android => android.OnApplicationCreate(app => UserDialogs.Init(app)));
#endif
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();

        }
    }
}

/*#if DEBUG
builder.Logging.AddDebug();
#endif*/