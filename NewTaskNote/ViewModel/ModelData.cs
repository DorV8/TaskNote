﻿
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

        public void SortNotes(int CategoryId)
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
            SelectedNotes = new ObservableCollection<NoteItem>(filtered);
        }
    }
}
