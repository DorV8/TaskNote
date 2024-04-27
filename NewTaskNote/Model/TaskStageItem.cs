using Commons;

namespace NewTaskNote
{
    public class TaskStageItem : BindableBase
    {
        public string TaskStageHeader { get; set; }
        public string TaskStageDesc { get; set; }

        public bool IsCompleted { get; set; }

        public TaskStageItem()
        {
        }
    }
}
