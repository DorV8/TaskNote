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
        public int CategoryId
        {
            get
            {
                var id = -1;
                switch (Category)
                {
                    case CategoryKind.Undefined:
                        id = -1;
                        break;
                    case CategoryKind.White:
                        id = 0;
                        break;
                    case CategoryKind.Green:
                        id = 1;
                        break;
                    case CategoryKind.Yellow:
                        id = 2;
                        break;
                    case CategoryKind.Red:
                        id = 3;
                        break;
                    case CategoryKind.Blue:
                        id = 4;
                        break;
                }
                return id;
            }
        }
        public Color ColorCategory
        {
            get
            {
                Color id = Color.FromRgb(255, 255, 255);
                switch (Category)
                {
                    case CategoryKind.Undefined:
                        id = Color.FromRgb(255, 255, 255);
                        break;
                    case CategoryKind.White:
                        id = Color.FromRgb(255, 255, 255);
                        break;
                    case CategoryKind.Green:
                        id = Color.FromRgb(0, 128, 0);
                        break;
                    case CategoryKind.Yellow:
                        id = Color.FromRgb(255, 255, 0);
                        break;
                    case CategoryKind.Red:
                        id = Color.FromRgb(255, 0, 0);
                        break;
                    case CategoryKind.Blue:
                        id = Color.FromRgb(0, 0, 255);
                        break;
                }
                return id;
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
