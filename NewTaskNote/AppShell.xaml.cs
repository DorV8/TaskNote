namespace NewTaskNote
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            App.Current.UserAppTheme = AppTheme.Light;
        }
    }
}
