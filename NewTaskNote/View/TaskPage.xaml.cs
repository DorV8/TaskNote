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
        var name = await DisplayPromptAsync("Создание задачи", "Введите название задачи:", "ОК", "Отмена");
        var desc = await DisplayPromptAsync("Создание задачи", "Введите описание задачи:", "ОК", "Отмена");
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

    private async void FinishTask_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Завершение", "Хотите завершить задачу?", "Да", "Нет");
        if (answer == true)
        {
            foreach (var stage in instanse.Data.EditedTask.AllStages)
            {
                stage.IsCompleted = true;
            }
            var index = instanse.Data.AllTasks.IndexOf(instanse.Data.CurrentTask);
            instanse.Data.AllTasks[index] = instanse.Data.EditedTask;
            Navigation.PopModalAsync();
        }
    }
}