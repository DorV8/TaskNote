using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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
                try
                {
                    instanse.Data.AddTask(item);
                    instanse.Data.database.AddTask(item);
                    var toast = Toast.Make("������ �������", ToastDuration.Short, 14);
                    toast.Show();
                }
                catch
                {
                    var toast = Toast.Make("���-�� ����� �� ���", ToastDuration.Short, 14);
                    toast.Show();
                }
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
        AddCategorys();
    }
    private void AddCategorys()
    {
        MenuPicker.Items.Clear();
        MenuPicker.Items.Add("��� ������");
        MenuPicker.Items.Add("���������");
    }
    private async void DeleteTaskMenuItem_Clicked(object sender, EventArgs e)
    {
        var param = ((MenuItem)sender).CommandParameter as TaskItem;
        var answer = await DisplayAlert("��������", "������ ������� ��� ������?", "��", "���");
        if (answer == true)
        {
            try
            {
                instanse.Data.RemoveTask(param);
                instanse.Data.database.RemoveTask(param);
            }
            catch (Exception ex)
            {
                var toast = Toast.Make("���-�� ����� �� ���", ToastDuration.Short, 14);
                Console.WriteLine(ex.ToString());
                toast.Show();
            }
            TasksList.ItemsSource = instanse.Data.OrderedAllTasks;
        }
    }

    private void MenuPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (MenuPicker.SelectedIndex)
        {
            case -1:
                TasksList.ItemsSource = instanse.Data.AllTasks;
                break;
            case 0:
                TasksList.ItemsSource = instanse.Data.AllTasks;
                break;
            case 1:
                instanse.Data.SortTasks();
                TasksList.ItemsSource = instanse.Data.SelectedTasks;
                break;
        }
    }
}