using Commons;

namespace NewTaskNote
{
    public enum CategoryKind { Undefined = -1, White, Green, Yellow, Red, Blue }
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

        public CategoryKind Category;

        public Color Color
        {
            get
            {
                Color result = Color.FromRgb(255, 255, 255);
                switch (Category)
                {
                    case CategoryKind.Undefined:
                        result = Color.FromRgb(255, 255, 255);
                        break;
                    case CategoryKind.White:
                        result = Color.FromRgb(255, 255, 255);
                        break;
                    case CategoryKind.Green:
                        result = Color.FromRgb(0, 128, 0);
                        break;
                    case CategoryKind.Yellow:
                        result = Color.FromRgb(255, 255, 0);
                        break;
                    case CategoryKind.Red:
                        result = Color.FromRgb(255, 0, 0);
                        break;
                    case CategoryKind.Blue:
                        result = Color.FromRgb(0, 0, 255);
                        break;
                }
                return result;
            }
        }

        public NoteItem()
        {
            ModDate = DateTime.Now;
        }

        public NoteItem(string text, CategoryKind category, bool isFavorite)
        {
            NoteText = text;
            Category = category;
            IsFavorite = isFavorite;
        }
    }
}
