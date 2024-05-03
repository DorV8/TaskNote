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
        if ((name != "") && (name != null))
        {
            var desc = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
            if (desc == null)
            {
            }
            else
            instanse.Data.AddTask(new TaskItem()
            {
                TaskHeader = name,
                TaskDesc = desc == "" ? "��� ��������" : desc,
                IsFavorite = false,
                IsAlarmed = false
            });
        }
    }

    private async void TasksList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            instanse.Data.CurrentTask = TasksList.SelectedItem as TaskItem;
            TasksList.SelectedItem = null;
            await Navigation.PushModalAsync(new TaskPage());
        }
    }

}