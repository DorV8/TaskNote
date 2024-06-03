using Commons;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;

namespace NewTaskNote
{
    public class NoteItem: BindableBase
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
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

        public CategoryNote Category = new();
        public Color Color
        {
            get
            {
                if (App.Current.UserAppTheme == AppTheme.Light)
                {
                    return Category.Color;
                }
                else
                {
                    if (App.Current.UserAppTheme == AppTheme.Dark)
                    {
                        return Category.NameCategory.Contains("Без") ? Color.FromArgb("#404040") : Category.Color;
                    }
                }
                return Color.FromArgb("#FFFFFF");
            }
        }

        public NoteItem()
        {
            ModDate = DateTime.Now;
            IsFavorite = false;
        }

        public NoteItem(string text, CategoryNote category, bool isFavorite)
        {
            NoteText = text;
            Category = category;
            IsFavorite = isFavorite;
        }
    }
}
