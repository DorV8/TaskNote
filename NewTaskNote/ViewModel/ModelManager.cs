
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
            AddSampleNotes();
            AddSampleTasks();
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
            foreach(var task in GetSampleTasks())
            {
                Data.AddTask(task);
            }
        }

        public List<TaskItem> GetSampleTasks()
        {
            /*List<TaskItem> result = [];

            for (var i = 0; i < 10; i++)
            {
                result.Add(new TaskItem()
                {
                    TaskHeader = String.Format("Название {0}", i),
                    TaskDesc = String.Format("Описание задачи под номером {0}",i)
                }
                );
            }

            return result;*/
            Data.database.InsertSamplesTasks();
            return Data.database.GetTasks();
        }
        public List<NoteItem> GetSampleNotes()
        {
            Data.database.InsertSamplesNotes();
            return Data.database.GetNotes();
        }
    }
}
