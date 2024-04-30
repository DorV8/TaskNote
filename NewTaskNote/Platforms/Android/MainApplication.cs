using Android.App;
using Android.Runtime;

namespace NewTaskNote
{
    [Application]
    public class MainApplication : MauiApplication
    {

        public static readonly string ChannelId = "notificationServiceChannel";
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
