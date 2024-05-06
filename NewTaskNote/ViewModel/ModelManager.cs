
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
            AddNotesSample();
            AddTasksSample();
        }

        private int notesSampleGroup = 0;

        public void AddNotesSample()
        {
            notesSampleGroup += 1;
            foreach (var note in GetSampleNotes())
            {
                Data.AddNote(note);
            }
        }

        public void AddTasksSample()
        {
            foreach(var task in GetSampleTasks())
            {
                Data.AddTask(task);
            }
        }

        public static List<TaskItem> GetSampleTasks()
        {
            List<TaskItem> result = [];

            for (var i = 0; i < 10; i++)
            {
                result.Add(new TaskItem()
                {
                    TaskHeader = String.Format("Название {0}", i),
                    TaskDesc = String.Format("Описание задачи под номером {0}",i)
                }
                );
            }

            return result;
        }
        public List<NoteItem> GetSampleNotes()
        {
            Data.database.InsertSamples();
            return Data.database.GetNotes();
        }
        public static List<NoteItem> GetSampleNotes(int groupIndex)
        {
            List<NoteItem> result = [];

            string longString = "Длинный предлинный текст который представляет из себя целый абзац и нужен для проверки.";

            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Обычная строка", groupIndex), new CategoryNote { ID = CategoryNote.CategoryNoteID.Work}, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Длинный текст", groupIndex), new CategoryNote { ID = CategoryNote.CategoryNoteID.Study }, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Очень длинный текст. Зеленая категория {1}", groupIndex, longString), new CategoryNote { ID = CategoryNote.CategoryNoteID.Home }, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Избранное", groupIndex), new CategoryNote { ID = CategoryNote.CategoryNoteID.Plans }, true));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Красная категория", groupIndex), new CategoryNote { ID = CategoryNote.CategoryNoteID.Purchases }, false));

            return result;
        }
    }
}
