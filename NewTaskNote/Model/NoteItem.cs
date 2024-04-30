using Commons;

namespace NewTaskNote
{
    public class NoteItem: BindableBase
    {
        private string _NoteText = "";
        public string NoteText
        {
            get { return _NoteText; }
            set { SetProperty(ref _NoteText, value); }
        }

        private DateTime _ModDate;
        public DateTime ModDate
        {
            get { return _ModDate; }
            set { SetProperty(ref _ModDate, value); }
        }

        public string FormattedDate { get { return String.Format("{0:dd.MM.yy}", ModDate); } }

        private bool _IsFavorite;
        public bool IsFavorite
        {
            get { return _IsFavorite; }
            set { SetProperty(ref _IsFavorite, value); }
        }

        public CategoryNote Category;
        public Color Color { get { return Category.Color; } }

        public NoteItem()
        {
            ModDate = DateTime.Now;
        }

        public NoteItem(string text, CategoryNote category, bool isFavorite)
        {
            NoteText = text;
            Category = category;
            IsFavorite = isFavorite;
        }
    }
}
