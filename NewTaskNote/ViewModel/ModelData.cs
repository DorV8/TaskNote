
using System.Collections.ObjectModel;

namespace NewTaskNote
{
    public class ModelData
    {
        public ObservableCollection<NoteItem> AllNotes { get; set; }

        public ObservableCollection<NoteItem> SelectedNotes { get; set; }

        public NoteItem CurrentNote { get; set; }
        public NoteItem EditedNote { get; set; }

        public ModelData()
        {
            AllNotes = new ObservableCollection<NoteItem>();
        }

        public void AddNote(NoteItem item)
        {
            AllNotes.Add(item);
        }

        public void RemoveNote(NoteItem item)
        {
            AllNotes.Remove(item);
        }
    }
}
