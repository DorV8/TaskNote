using Commons;
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class TaskItem: BindableBase
    {
        public int id;
        public DateTime ModDate { get; set; }

        public string TaskHeader { get; set; }
        public string TaskDesc { get; set; }

        public bool IsFinished
        {
            get { return StageCount == 0 ? false : StageProgress == StageCount; }
        }

        public Color bgColor
        {
            get
            {
                if (IsFinished)
                {
                    return Color.FromArgb("#409945");
                }
                else
                {
                    if (App.Current.UserAppTheme == AppTheme.Light)
                    {
                        return Color.FromArgb("#FFFFFF");
                    }
                    else
                    {
                        return Color.FromArgb("404040");
                    }
                }
                return (IsFinished == false)  ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#98FB98");
            }
        }
        public bool IsFavorite { get; set; }

        public ObservableCollection<TaskStageItem> AllStages { get; set; }

        public int StageCount { get { return AllStages.Count; } }

        public int StageProgress 
        {
            get { return AllStages.Count(stage => stage.IsCompleted); }
        }

        public DateTime AlarmDate { get; set; }

        public bool IsAlarmed { get; set; }

        public TaskStageItem CurrentStage { get; set; }

        public TaskItem()
        {
            AllStages = [];
        }

        public void AddStage(TaskStageItem item)
        {
            AllStages.Add(item);
        }

        public void RemoveStage(TaskStageItem item)
        {
            AllStages.Remove(item);
        }
    }
}
