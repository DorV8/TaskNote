
using System.Xml.Linq;

namespace NewTaskNote
{
    public class CategoryNote
    {
        private DatabaseContext database = new();
        public enum CategoryNoteID { Work = 1, Study = 2, Home = 3, Purchases = 4, Plans = 5, NoCategory = 6 }

        public CategoryNoteID ID { get; set; }
        public string NameCategory
        {
            get
            {
                string result = database.GetCategoryNameById(((int)ID));
                return result;
                /*var Name = "";
                switch (ID)
                {
                    case CategoryNoteID.Undefined:
                        Name = "Нет категории";
                        break;
                    case CategoryNoteID.Work:
                        Name = "Работа";
                        break;
                    case CategoryNoteID.Study:
                        Name = "Учёба";
                        break;
                    case CategoryNoteID.Home:
                        Name = "Домашние дела";
                        break;
                    case CategoryNoteID.Purchases:
                        Name = "Покупки";
                        break;
                    case CategoryNoteID.Plans:
                        Name = "Планы";
                        break;
                }
                return Name;*/
            }
        }
        public Color Color
        {
            get
            {
                var result = database.GetCategoryColorById((int)ID);
                return result;
                
            }/*var result = Color.FromRgb(255, 255, 255);
                switch (ID)
                {
                    case CategoryNoteID.Undefined:
                        result = Color.;
                        break;
                    case CategoryNoteID.Work:
                        result = "Работа";
                        break;
                    case CategoryNoteID.Study:
                        result = "Учёба";
                        break;
                    case CategoryNoteID.Home:
                        result = "Домашние дела";
                        break;
                    case CategoryNoteID.Purchases:
                        result = "Покупки";
                        break;
                    case CategoryNoteID.Plans:
                        result = "Планы";
                        break;
                }
                return result;*/

        }
    }
}
