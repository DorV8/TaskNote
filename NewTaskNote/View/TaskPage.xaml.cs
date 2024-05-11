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
        var task = new TaskItem()
        {
            TaskHeader = instanse.Data.CurrentTask.TaskHeader,
            TaskDesc = instanse.Data.CurrentTask.TaskDesc,
            AllStages = instanse.Data.CurrentTask.AllStages,
            AlarmDate = instanse.Data.CurrentTask.AlarmDate,
            CurrentStage = instanse.Data.CurrentTask.CurrentStage,
            IsAlarmed = instanse.Data.CurrentTask.IsAlarmed,
            IsFavorite = instanse.Data.CurrentTask.IsFavorite,
            ModDate = instanse.Data.CurrentTask.ModDate
        };
        instanse.Data.EditedTask = task;

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
        catch { }
    }
    private async void CheckChanges()
    {
        if ((editedTask.TaskHeader != currentTask.TaskHeader) || (editedTask.TaskDesc != currentTask.TaskDesc))
        {
            var answer = await DisplayAlert("���������", "������ ��������� ���������?", "��", "���");
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
        //����� ����� ������ � ��
    }

    private async void FinishTask_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("����������", "������ ��������� ������?", "��", "���");
        if (answer == true)
        {
            foreach (var stage in editedTask.AllStages) { stage.IsCompleted = true; }
            SetCurrentTask();
            Navigation.PopModalAsync();
        }
    }

    private void SetFavorite()
    {
        FavoriteButton.Text = editedTask.IsFavorite ? "������ �� ����������" : "�������� � ���������";
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
        AlarmedButton.Text = editedTask.IsAlarmed ? "������ �����������" : "�������� �����������";
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
            Title = "������� ������� ���� ������",
            Subtitle = "�����������",
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

    private void HeaderEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Console.WriteLine("editedTask.TaskHeader = " + editedTask.TaskHeader);
        Console.WriteLine("currentTask.TaskHeader = " + currentTask.TaskHeader);
    }

    private async void DeleteStageButton_Clicked(object sender, EventArgs e)
    {
        var item = ((Button)sender).BindingContext;
        var answer = await DisplayAlert("��������", "������ ������� ��� ���������?", "��", "���");
        if (answer == true)
        {
            instanse.Data.EditedTask.AllStages.Remove(item as TaskStageItem);
        }
    }
}