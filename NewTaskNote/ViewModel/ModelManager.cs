
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class ModelManager
    {
        private static ModelManager singletoneInstanse = new();

        public static ModelManager GetInstanse()
        {
            return singletoneInstanse;
        }

        public ModelData Data { get; private set; }

        public ModelManager()
        {
            Data = new ModelData();
        }
        public void AddSampleNotes()
        {
            foreach (var note in GetSampleNotes())
            {
                Data.AddNote(note);
            }
        }

        public void AddSampleTasks()
        {
            foreach (var task in GetSampleTasks())
            {
                Data.AddTask(task);
            }
        }
        private ObservableCollection<NoteItem> GetSampleNotes()
        {
            Random rnd = new Random();
            ObservableCollection<NoteItem> result = [];
            for (var i = 0; i < 5; i++)
            {
                var id = rnd.Next(1, 6);
                result.Add(new NoteItem()
                {
                    id = -1,
                    NoteText = String.Format("демонстрационная заметка №{0}", i+1),
                    ModDate = DateTime.Now,
                    IsFavorite = (id % 2) == 0,
                    Category = new CategoryNote()
                    {
                        ID = id,
                        NameCategory = Data.categorys[id].NameCategory,
                        Color = Data.categorys[id].Color
                    }
                });
            }
            return result;
        }

        public ObservableCollection<TaskItem> GetSampleTasks()
        {
            ObservableCollection<TaskItem> result = [];

            for (var i = 0; i < 10; i++)
            {
                TaskItem item = new()
                {
                    id = -1,
                    TaskHeader = String.Format("Название {0}", i),
                    TaskDesc = String.Format("Описание задачи под номером {0}", i),
                    IsFavorite = false,
                    ModDate = DateTime.Now,
                    IsAlarmed = false
                };
                for (int j = 1; j <=3; j++)
                {
                    item.AllStages.Add(getSampleStage(i, j));
                }
                result.Add(item);
            }
            return result;
        }
        private TaskStageItem getSampleStage(int number_task, int number_stage)
        {
            Random rnd = new();
            TaskStageItem item = new()
            {
                id = -1,
                TaskStageHeader = String.Format("Этап №{0} задачи №{1}", number_stage, number_task),
                TaskStageDesc = String.Format("Описание этапа №{0} задачи №{1}", number_stage, number_task),
                IsCompleted = false
            };
            return item;

        }
    }
}
