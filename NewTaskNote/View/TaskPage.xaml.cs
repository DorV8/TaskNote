namespace NewTaskNote;

public partial class TaskPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public TaskItem currentTask;
    public TaskItem editedTask;

    public TaskPage()
	{
		InitializeComponent();

        instanse.Data.EditedTask = instanse.Data.CurrentTask;
        
        currentTask = instanse.Data.CurrentTask;
        editedTask = instanse.Data.EditedTask;

        SetFavorite();

        this.BindingContext = editedTask;
        StagesList.ItemsSource = editedTask.AllStages;

    }

    private async void AddStageButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        try
        {
            if (name != null)
            {
                var desc = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
                editedTask.AllStages.Add(new TaskStageItem()
                {
                    TaskStageHeader = name,
                    TaskStageDesc = desc
                });
            }
        }
        catch {}
    }
    protected override bool OnBackButtonPressed()
    {
        SetCurrentTask();

        Navigation.PopModalAsync();
        return true;
    }

    private async void FinishTask_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("����������", "������ ��������� ������?", "��", "���");
        if (answer == true)
        {
            foreach (var stage in editedTask.AllStages)
            {
                stage.IsCompleted = true;
            }
            SetCurrentTask();
            Navigation.PopModalAsync();
        }
    }

    private void SetFavorite()
    {
        FavoriteButton.Text = editedTask.IsFavorite ? "������ �� ����������" : "�������� � ���������";
    }
    private void SetCurrentTask()
    {
        var index = instanse.Data.AllTasks.IndexOf(currentTask);
        instanse.Data.AllTasks[index] = editedTask;
    }

    private void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        editedTask.IsFavorite = !editedTask.IsFavorite;
        SetFavorite();
    }
}