using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTaskNote
{
    public class CategoryNote
    {
        public enum CategoryNoteID { Undefined = -1, Red, Green, Blue}

        public CategoryNoteID ID { get; set; }
        public string NameColor
        {
            get
            {
                var color = "";
                switch (ID)
                {
                    case CategoryNoteID.Undefined:
                        color = "Нет цвета";
                        break;
                    case CategoryNoteID.Red:
                        color = "Красный";
                        break;
                    case CategoryNoteID.Green:
                        color = "Зелёный";
                        break;
                    case CategoryNoteID.Blue:
                        color = "Синий";
                        break;
                }
                return color;
            }
        }

        public Color Color
        {
            get
            {
                var result = Color.FromRgb(255, 255, 255);
                switch (ID)
                {
                    case CategoryNoteID.Red:
                        result = Color.FromRgb(255, 0, 0);
                        break;
                    case CategoryNoteID.Green:
                        result = Color.FromRgb(0, 255, 0);
                        break;
                    case CategoryNoteID.Blue:
                        result = Color.FromRgb(0, 0, 255);
                        break;
                }
                return result;
            }

        }
    }
}
