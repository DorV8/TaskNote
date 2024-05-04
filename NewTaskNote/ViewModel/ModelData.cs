
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class ModelData
    {
        private static ObservableCollection<NoteItem> GetReverse(ObservableCollection<NoteItem> collection)
        {
            var result = new ObservableCollection<NoteItem>();

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                result.Add(collection[i]);
            }

            return result;
        }

        public ObservableCollection<NoteItem> AllNotes { get; set; }
        public ObservableCollection<NoteItem> ReversedAllNotes
        {
            get
            {
                return GetReverse(AllNotes);
            }
        }

        public ObservableCollection<NoteItem> SelectedNotes { get; set; }
        public ObservableCollection<NoteItem> ReversedSelectedNotes
        {
            get { return GetReverse(SelectedNotes); }
        }

        public NoteItem CurrentNote { get; set; }
        public NoteItem EditedNote { get; set; }
        
        //****************************************************

        public ObservableCollection<TaskItem> AllTasks { get; set; }
        
        public TaskItem CurrentTask { get; set; }

        public TaskItem EditedTask { get; set; }

        //****************************************************

        public ModelData()
        {
            AllNotes = [];
            AllTasks = [];
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

        public void RewriteNote(NoteItem currentNote, NoteItem newNote)
        {
            var index = AllNotes.IndexOf(currentNote);
            AllNotes[index] = newNote;
        }

        public void SortNotes(CategoryNote.CategoryNoteID ID, string TextPiece)
        {
            IEnumerable<NoteItem> filtered = null;
            filtered = AllNotes.Where(note => note.Category.ID == ID);
             
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
