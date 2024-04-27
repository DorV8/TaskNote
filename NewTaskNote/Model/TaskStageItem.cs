using Commons;

namespace NewTaskNote
{
    public class TaskStageItem : BindableBase
    {
        public string TaskStageHeader { get; set; }
        public string TaskStageDesc { get; set; }
        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                SetProperty(ref _IsCompleted, value);
            }
        }

        public TaskStageItem()
        {
        }
    }
}
