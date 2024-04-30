
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

        public TaskItem EditedTask { get; set; }

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

        public void SortNotes(CategoryNote.CategoryNoteID ID, string TextPiece)
        {
            IEnumerable<NoteItem> filtered = null;
            filtered = AllNotes.Where(note => note.Category.ID == ID);
            /*switch (CategoryId)
            {
                case -1:
                    filtered = AllNotes;
                    break;
                case 0:
                    filtered = AllNotes.Where(note => note.Category == CategoryNote.CategoryNoteID.White);
                    break;
                case 1:
                    filtered = AllNotes.Where(note => note.Category == CategoryNote.CategoryNoteID.Green);
                    break;
                case 2:
                    filtered = AllNotes.Where(note => note.Category == CategoryNote.CategoryNoteID.Yellow);
                    break;
                case 3:
                    filtered = AllNotes.Where(note => note.Category == CategoryNote.CategoryNoteID.Red);
                    break;
                case 4:
                    filtered = AllNotes.Where(note => note.Category == CategoryNote.CategoryNoteID.Blue);
                    break;
                case 5:
                    filtered = AllNotes.Where(note => note.IsFavorite == true);
                    break;
            }*/
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
    }
}
