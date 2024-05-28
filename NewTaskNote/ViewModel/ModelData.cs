﻿
using System.Collections.ObjectModel;
using Font = Microsoft.Maui.Font;

namespace NewTaskNote
{
    public class ModelData
    {
        public DatabaseContext database;

        //****************************************************

        public List<Font> fonts = [];//добавить шрифты

        private void SetFonts()
        {
        }

        public List<string> fontNames
        {
            get
            {
                var result = new List<string>();
                foreach (var font in fonts)
                {
                    result.Add(font.Family);
                }
                return result;
            }
        }

        //****************************************************
        public List<CategoryNote> categorys { get; private set; }
        private static ObservableCollection<NoteItem> GetReverse(ObservableCollection<NoteItem> collection)
        {
            var result = new ObservableCollection<NoteItem>();

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                result.Add(collection[i]);
            }

            return result;
        }
        private void SetCategorys()
        {
            categorys = database.GetCategorys();
        }

        //****************************************************

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

        public ObservableCollection<TaskItem> OrderedAllTasks
        {
            get
            {
                return new ObservableCollection<TaskItem>(AllTasks.OrderBy(c => c.IsFinished));
            }
        }
        
        public TaskItem CurrentTask { get; set; }

        public TaskItem EditedTask { get; set; }

        //****************************************************

        public ModelData()
        {
            database = new();
            AllNotes = [];
            GetNotes();
            SetCategorys();
            AllTasks = [];
            GetTasks();
        }

        //****************************************************
        public void GetNotes()
        {
            AllNotes = database.GetNotes();
        }
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

        public void SortNotes(int ID, string TextPiece)
        {
            IEnumerable<NoteItem> filtered = null;
            if (categorys.Count-1 < ID)
            {
                filtered = AllNotes.Where(note => note.IsFavorite == true);
            }
            else
            {
                filtered = AllNotes.Where(note => note.Category.ID == ID);
            }
             
            if (TextPiece != "")
            {
                filtered = filtered.Where(note => note.NoteText.ToUpper().Contains(TextPiece.ToUpper()));
            }
            SelectedNotes = new ObservableCollection<NoteItem>(filtered);
        }

        //****************************************************
        public void GetTasks()
        {
            AllTasks = database.GetTasks();
        }
        public void AddTask(TaskItem item)
        {
            AllTasks.Add(item);
        }

        public TaskItem CopyTask(TaskItem item)
        {
            var task = new TaskItem()
            {
                id = item.id,
                TaskHeader = item.TaskHeader,
                TaskDesc = item.TaskDesc,
                AllStages = item.AllStages,
                AlarmDate = item.AlarmDate,
                CurrentStage = item.CurrentStage,
                IsAlarmed = item.IsAlarmed,
                IsFavorite = item.IsFavorite,
                ModDate = item.ModDate
            };
            return task;
        }

        public void RemoveTask(TaskItem item)
        {
            AllTasks.Remove(item);
            OrderedAllTasks.Remove(item);
        }

        public void RewriteTask(TaskItem oldItem, TaskItem newItem)
        {
            var index = AllTasks.IndexOf(oldItem);
            AllTasks[index] = newItem;
        }
    }
}
