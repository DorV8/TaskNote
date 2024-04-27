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
}