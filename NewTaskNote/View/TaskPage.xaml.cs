using Acr.UserDialogs;
using Microsoft.Maui.Handlers;
using Plugin.LocalNotification;

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
        SetDate();

        this.BindingContext = editedTask;
        StagesList.ItemsSource = editedTask.AllStages;

        AlarmedDatePicker.IsVisible = false;

    }

    private async void AddStageButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Создание задачи", "Введите название задачи:", "ОК", "Отмена");
        try
        {
            if (name != null)
            {
                var desc = await DisplayPromptAsync("Создание задачи", "Введите описание задачи:", "ОК", "Отмена");
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
        var answer = await DisplayAlert("Завершение", "Хотите завершить задачу?", "Да", "Нет");
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
        FavoriteButton.Text = editedTask.IsFavorite ? "Убрать из избранного" : "Добавить в избранное";
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

    private void AlarmedButton_Clicked(object sender, EventArgs e)
    {
        if (editedTask.IsAlarmed == false)
        {
            ShowDatePicker();
            SetDate();

        }
        else
        {
            editedTask.IsAlarmed = false;
            SetDate();
        }
    }

    private void ShowDatePicker()
    {
        AlarmedDatePicker.MinimumDate = DateTime.Now;
        var handler = AlarmedDatePicker.Handler as IDatePickerHandler;
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        handler.PlatformView.PerformClick();
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
        SetDate(true);
    }

    private void SetDate()
    {
        AlarmedButton.Text = editedTask.IsAlarmed ? "Убрать напоминание" : "Добавить напоминание";
        AlarmedDatePicker.Date = editedTask.IsAlarmed ? editedTask.AlarmDate : DateTime.MinValue;
        AlarmedDatePicker.IsVisible = editedTask.IsAlarmed;
    }

    private void SetDate(bool res)
    {
        if (res)
        {
            editedTask.AlarmDate = AlarmedDatePicker.Date;
            editedTask.IsAlarmed = true;
            //AlarmedText.Text = editedTask.AlarmDate.ToString("dd:mm:yy");
            SetDate();

            var request = new NotificationRequest
            {
                NotificationId = 1337,
                Title = "Сегодня крайний срок задачи",
                Subtitle = "Напоминание",
                Description = editedTask.TaskHeader,
                BadgeNumber = 1,

                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = GetDateFromTask().AddSeconds(5)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }
    }

    private DateTime GetDateFromTask()
    {
        return
            editedTask.AlarmDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
    } 
}