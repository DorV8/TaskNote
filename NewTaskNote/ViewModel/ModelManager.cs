using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTaskNote
{
    public class ModelManager
    {
        private static ModelManager singletoneInstanse = new ModelManager();

        public static ModelManager GetInstanse()
        {
            return singletoneInstanse;
        }

        public ModelData Data { get; private set; }

        public ModelManager()
        {
            Data = new ModelData();
            AddNotesSample();
        }

        private int notesSampleGroup = 0;

        public void AddNotesSample()
        {
            notesSampleGroup += 1;
            foreach (var note in GetSampleNotes(notesSampleGroup))
            {
                Data.AddNote(note);
            }
        }

        public List<NoteItem> GetSampleNotes(int groupIndex)
        {
            List<NoteItem> result = new List<NoteItem>();

            string longString = "Длинный предлинный текст который представляет из себя целый абзац и нужен для проверки.";

            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Обычная строка", groupIndex), CategoryKind.White, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Длинный текст", groupIndex), CategoryKind.White, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Очень длинный текст. Зеленая категория {1}", groupIndex, longString), CategoryKind.Green, false));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Избранное", groupIndex), CategoryKind.White, true));
            result.Add(new NoteItem(String.Format("Записка в группе [{0}]. Красная категория", groupIndex), CategoryKind.Red, false));

            return result;
        }
    }
}
