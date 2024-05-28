using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Shiny;

namespace NewTaskNote
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseShiny()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })

                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    //events.AddAndroid(android => android.OnApplicationCreate(app => UserDialogs.Init(app)));
#endif
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();

        }
    }
}