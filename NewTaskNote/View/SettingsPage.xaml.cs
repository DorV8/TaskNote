namespace NewTaskNote;

public partial class SettingsPage : ContentPage
{
    ModelManager instanse = ModelManager.GetInstanse();
    bool notesAdded = false;
    bool tasksAdded = false;
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		if (ThemePicker.Items.Count == 0)
		{
            var themes = new List<string>(
            [
            "Тёмная тема",
            "Светлая тема",
            "Как в системе"
            ]);
            foreach (string theme in themes)
            {
                ThemePicker.Items.Add(theme);
            }
        }
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
		App.Current.UserAppTheme = ThemePicker.SelectedIndex == 0 ? AppTheme.Dark : ThemePicker.SelectedIndex == 1 ? AppTheme.Light : AppTheme.Unspecified;
    }

    private void InsertSamplesNotes_Clicked(object sender, EventArgs e)
    {
        instanse.AddSampleNotes();
        InsertSamplesNotes.IsEnabled = false;
    }

    private void InsertSamplesTasks_Clicked(object sender, EventArgs e)
    {
        instanse.AddSampleTasks();
        InsertSamplesTasks.IsEnabled = false;
    }

    private void ClearDB_button_Clicked(object sender, EventArgs e)
    {
        try
        {
            instanse.Data.database.ClearData();
            instanse.Data.ClearData();
        }
        catch
        {
            Console.WriteLine("Ошибка при очищении БД");
        }
    }
}