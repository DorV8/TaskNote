using Commons;
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class TaskItem: BindableBase
    {
        public DateTime ModDate { get; set; }

        public string TaskHeader { get; set; }
        public string TaskDesc { get; set; }

        public bool IsFinished
        {
            get { return StageProgress == StageCount; }
        }
        public bool IsFavorite { get; set; }

        public ObservableCollection<TaskStageItem> AllStages { get; set; }

        public int StageCount { get { return AllStages.Count; } }

        public int StageProgress 
        {
            get { return AllStages.Count(stage => stage.IsCompleted); }
            set { StageProgress = value; }
        }

        public DateTime AlarmDate { get; set; }

        public bool IsAlarmed { get; set; }

        public TaskStageItem CurrentStage { get; set; }

        public TaskItem()
        {
            AllStages = new ObservableCollection<TaskStageItem>();
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
