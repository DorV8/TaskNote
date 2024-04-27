namespace NewTaskNote;

public partial class TasksPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public TasksPage()
	{
		InitializeComponent();

		this.BindingContext = instanse.Data;
		TasksList.ItemsSource = instanse.Data.AllTasks;
	}

    private async void AddTaskButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        var desc = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        try
        {
            if (name != null)
            {
                instanse.Data.AddTask(new TaskItem()
                { 
                    TaskHeader = name,
                    TaskDesc = desc
                });
            }
        }
        catch
        {

        }
    }
}