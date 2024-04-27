
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class ModelData
    {
        public ObservableCollection<NoteItem> AllNotes { get; set; }

        public ObservableCollection<NoteItem> SelectedNotes { get; set; }

        public NoteItem CurrentNote { get; set; }
        public NoteItem EditedNote { get; set; }
        
        //****************************************************

        public ObservableCollection<TaskItem> AllTasks { get; set; }
        
        public TaskItem CurrentTask { get; set; }

        //****************************************************

        public ModelData()
        {
            AllNotes = new ObservableCollection<NoteItem>();
            AllTasks = new ObservableCollection<TaskItem>();
        }

        //****************************************************

        public void AddNote(NoteItem item)
        {
            AllNotes.Add(item);
        }

        public void RemoveNote(NoteItem item)
        {
            AllNotes.Remove(item);
        }

        public void SortNotes(int CategoryId, string TextPiece)
        {
            IEnumerable<NoteItem> filtered = null;
            switch (CategoryId)
            {
                case -1:
                    filtered = AllNotes;
                    break;
                case 0:
                    filtered = AllNotes.Where(note => note.Category == CategoryKind.White);
                    break;
                case 1:
                    filtered = AllNotes.Where(note => note.Category == CategoryKind.Green);
                    break;
                case 2:
                    filtered = AllNotes.Where(note => note.Category == CategoryKind.Yellow);
                    break;
                case 3:
                    filtered = AllNotes.Where(note => note.Category == CategoryKind.Red);
                    break;
                case 4:
                    filtered = AllNotes.Where(note => note.Category == CategoryKind.Blue);
                    break;
                case 5:
                    filtered = AllNotes.Where(note => note.IsFavorite == true);
                    break;
            }
            if (TextPiece != "")
            {
                filtered = filtered.Where(note => note.NoteText.ToUpper().Contains(TextPiece.ToUpper()));
            }
            SelectedNotes = new ObservableCollection<NoteItem>(filtered);
        }

        //****************************************************

        public void AddTask(TaskItem item)
        {
            AllTasks.Add(item);
        }

        public void RemoveTask(TaskItem item)
        {
            AllTasks.Remove(item);
        }

        public void AddStage(TaskStageItem item)
        {
            CurrentTask.AllStages.Add(item);
        }
        public void RemoveStage(TaskStageItem item)
        {
            CurrentTask.AllStages.Remove(item);
        }
    }
}
