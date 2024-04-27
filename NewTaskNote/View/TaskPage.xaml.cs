namespace NewTaskNote;

public partial class TaskPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public TaskPage()
	{
		InitializeComponent();

        instanse.Data.EditedTask = instanse.Data.CurrentTask;

        this.BindingContext = instanse.Data.EditedTask;
        StagesList.ItemsSource = instanse.Data.CurrentTask.AllStages;
	}

    private async void AddStageButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        var desc = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        try
        {
            if (name != null)
            {
                instanse.Data.CurrentTask.AllStages.Add(new TaskStageItem()
                {
                    TaskStageHeader = name,
                    TaskStageDesc = desc
                });
            }
        }
        catch
        {

        }
    }
    protected override bool OnBackButtonPressed()
    {
        var index = instanse.Data.AllTasks.IndexOf(instanse.Data.CurrentTask);
        instanse.Data.AllTasks[index] = instanse.Data.EditedTask;

        Navigation.PopModalAsync();
        return true;
    }
}