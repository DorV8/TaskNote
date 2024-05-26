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
        instanse.Data.EditedTask = instanse.Data.CopyTask(instanse.Data.CurrentTask);

        currentTask = instanse.Data.CurrentTask;
        editedTask = instanse.Data.EditedTask;

        SetFavorite();
        SetDate();

        this.BindingContext = editedTask;
        StagesList.ItemsSource = editedTask.AllStages;

        AlarmedDatePicker.IsVisible = editedTask.IsAlarmed;
    }

    private async void AddStageButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Создание задачи", "Введите название задачи:", "ОК", "Отмена");
        try
        {
            if ((name !="") && (name != null))
            {
                var desc = await DisplayPromptAsync("Создание задачи", "Введите описание задачи:", "ОК", "Отмена");
                var item = new TaskStageItem()
                {
                    TaskStageHeader = name,
                    TaskStageDesc = desc
                };
                editedTask.AddStage(item);
                instanse.Data.database.AddStage(item, currentTask.id);
            }
        }
        catch { }
    }
    private async void CheckChanges()
    {
        if ((editedTask.TaskHeader != currentTask.TaskHeader) || (editedTask.TaskDesc != currentTask.TaskDesc))
        {
            var answer = await DisplayAlert("Изменения", "Хотите сохранить изменения?", "Да", "Нет");
            if (answer == false)
            {
                editedTask.TaskHeader = currentTask.TaskHeader;
                editedTask.TaskDesc = currentTask.TaskDesc;
            }
        }
        SetCurrentTask();
        Navigation.PopModalAsync();

    }
    protected override bool OnBackButtonPressed()
    {
        CheckChanges();
        return true;
    }
    private void SetCurrentTask()
    {
        instanse.Data.RewriteTask(currentTask, editedTask);
        instanse.Data.database.RewriteTask(currentTask.id, editedTask);
    }

    private async void FinishTask_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Завершение", "Хотите завершить задачу?", "Да", "Нет");
        if (answer == true)
        {
            foreach (var stage in editedTask.AllStages) { stage.IsCompleted = true; }
            SetCurrentTask();
            Navigation.PopModalAsync();
        }
    }

    private void SetFavorite()
    {
        FavoriteButton.Source = editedTask.IsFavorite ? "favorite.png" : "not_favorite.png";
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
        }
        else
        {
            editedTask.IsAlarmed = false;
        }
        SetDate();
    }

    private void ShowDatePicker()
    {
    #if ANDROID
        AlarmedDatePicker.MinimumDate = DateTime.Now;
        var handler = AlarmedDatePicker.Handler as IDatePickerHandler;
        handler.PlatformView.PerformClick();
        SetNotification();
    #endif
    }

    private void SetDate()
    {
        AlarmedButton.Text = editedTask.IsAlarmed ? "Убрать напоминание" : "Добавить напоминание";
        AlarmedDatePicker.Date = editedTask.IsAlarmed ? editedTask.AlarmDate : DateTime.MinValue;
        AlarmedDatePicker.IsVisible = editedTask.IsAlarmed;
    }

    private void SetNotification()
    {
        editedTask.AlarmDate = AlarmedDatePicker.Date;
        editedTask.IsAlarmed = true;
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

    private DateTime GetDateFromTask()
    {
        return editedTask.AlarmDate.AddHours(DateTime.Now.Hour)
                                .AddMinutes(DateTime.Now.Minute)
                                .AddSeconds(DateTime.Now.Second);
    }

    private async void DeleteStageButton_Clicked(object sender, EventArgs e)
    {
        var item = ((ImageButton)sender).BindingContext as TaskStageItem;
        var answer = await DisplayAlert("Удаление", "Хотите удалить эту подзадачу?", "Да", "Нет");
        if (answer == true)
        {
            editedTask.RemoveStage(item);
            instanse.Data.database.RemoveStage(item.id);
        }
    }
}