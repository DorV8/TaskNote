namespace NewTaskNote;

public partial class TasksPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public TasksPage()
	{
		InitializeComponent();
		this.BindingContext = instanse.Data;
        TasksList.ItemsSource = instanse.Data.OrderedAllTasks;
    }

    private async void AddTaskButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
        if ((name != "") && (name != null))
        {
            var desc = await DisplayPromptAsync("�������� ������", "������� �������� ������:", "��", "������");
            if (desc != null)
            {
                var item = new TaskItem()
                {
                    TaskHeader = name,
                    TaskDesc = desc == "" ? "��� ��������" : desc,
                    IsFavorite = false,
                    IsAlarmed = false
                };
                instanse.Data.AddTask(item);
                instanse.Data.database.AddTask(item);
                TasksList.ItemsSource = instanse.Data.OrderedAllTasks;
            }
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

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        TasksList.ItemsSource = instanse.Data.OrderedAllTasks;
    }

    private async void DeleteTaskMenuItem_Clicked(object sender, EventArgs e)
    {
        var param = ((MenuItem)sender).CommandParameter as TaskItem;
        var answer = await DisplayAlert("��������", "������ ������� ��� ������?", "��", "���");
        if (answer == true)
        {
            instanse.Data.RemoveTask(param);
            instanse.Data.database.RemoveTask(param);
            TasksList.ItemsSource = instanse.Data.OrderedAllTasks;
        }
    }
}