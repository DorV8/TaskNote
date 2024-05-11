using Microsoft.Data.Sqlite;

namespace NewTaskNote
{
    public class DatabaseContext
    {
        private const string DbName = "TaskNoteDatabase.db3";
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

        public DatabaseContext()
        {
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            //File.Delete(DbPath);
            CreateCategorys();
            CreateNotes();

            CreateTasks();
            CreateStages();
        }

        private void CreateTable(string commandString)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            using var command = new SqliteCommand(commandString, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        //*******************************************************
        private void CreateCategorys()
        {
            var commandString =
                "CREATE TABLE IF NOT EXISTS Categorys " +
                "(Id INTEGER PRIMARY KEY," +
                "Category_name TEXT," +
                "Category_color TEXT)";
            CreateTable(commandString);
            Console.WriteLine("Таблица категорий успешно создана");

            AddCategorys();
        }

        private static bool IsCategorysEmpty()
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var command = new SqliteCommand("SELECT count(Id) FROM Categorys", connection);
            var count = Convert.ToInt64(command.ExecuteScalar());
            return count == 0 ? true: false;
        }

        private void AddCategorys()
        {
            if (IsCategorysEmpty())
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();
                var names = new List<string>()
                {
                    "Работа",
                    "Учёба",
                    "Домашние дела",
                    "Покупки",
                    "Планы",
                    "Без категории"
                };
                var colors = new List<string>()
                {
                    "#e45f2b", //red
                    "#f6c445", //yellow
                    "#9ac1f0", //blue
                    "#72D293", //green 
                    "#8e65ab", //purple
                    "#FFFFFF" //white
                };
                var commandString = "INSERT INTO Categorys (Category_name, Category_color)" +
                                    "VALUES (@name, @color)";
                using var command = new SqliteCommand(commandString, connection);
                for (var i = 0; i < colors.Count; i++)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(new[]
                    {
                        new SqliteParameter("@name", names[i]),
                        new SqliteParameter("@color", colors[i])
                    });
                    command.ExecuteNonQuery();
                }
            }
        }

        //*******************************************************

        private void CreateNotes()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS Notes " +
                "(" +
                    "Id_note INTEGER PRIMARY KEY," +
                    "Note_text TEXT," +
                    "isFavorite BOOLEAN," +
                    "id_category INTEGER," +
                    "ModDate DATE," +
                    "FOREIGN KEY (id_category) REFERENCES Categorys(Id)" +
                ")";
            CreateTable(commandString);
            Console.WriteLine("Таблица заметок успешно создана");
        }

        public void InsertSamples()
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            using var clearCommand = new SqliteCommand("DELETE FROM Notes WHERE Id_note>0", connection);
            clearCommand.ExecuteNonQuery();

            var commandString = "INSERT INTO Notes (Note_text, isFavorite, id_category, ModDate) VALUES (@text, @fav, @id, @date)";
            Random rnd = new();
            using var command = new SqliteCommand(commandString, connection);
            for (int i = 0; i < 6; i++)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(new[]
                {
                    new SqliteParameter("@text", "заметка " + i),
                    new SqliteParameter("@fav", true),
                    new SqliteParameter("@id", rnd.Next(1,6)),
                    new SqliteParameter("@date", DateTime.Now)
                });
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public List<NoteItem> GetNotes()
        {
            List<NoteItem> result = [];
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "SELECT * FROM Notes";
            using var command = new SqliteCommand(commandString, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                NoteItem note = new() 
                {
                    id = reader.GetInt32(0),
                    NoteText = reader.GetString(1),
                    IsFavorite = reader.GetBoolean(2),
                    Category = new CategoryNote()
                    {
                        ID = reader.GetInt32(3),
                        NameCategory = GetCategoryNameById(reader.GetInt32(3)),
                        Color = GetCategoryColorById(reader.GetInt32(3))
                    }
                };
                /*note.id = reader.GetInt32(0);
                note.NoteText = reader.GetString(1);
                note.IsFavorite = reader.GetBoolean(2);
                note.Category = new CategoryNote() { ID = reader.GetInt32(3) };
                note.Category.NameCategory = GetCategoryNameById(note.Category.ID);
                note.Category.Color = GetCategoryColorById(note.Category.ID);*/
                result.Add(note);
            }
            connection.Close();

            return result;
        }

        //*******************************************************

        public void CreateTasks()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS Tasks " +
                "(" +
                    "Id INT PRIMARY KEY," +
                    "ModDate DATE," +
                    "Header TEXT," +
                    "Desc TEXT," +
                    "IsFavorite BOOL," +
                    "AlarmDate DATE" +
               ")";
            CreateTable(commandString);
        }

        public void AddTask(TaskItem item)
        {
            //
        }

        public void RemoveTask(TaskItem item)
        {

        }

        public void RewriteTask(TaskItem oldItem, TaskItem newItem)
        {

        }
        //*******************************************************

        public void CreateStages()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS Stages" +
                "(" +
                    "Id INT PRIMARY KEY," +
                    "Header TEXT," +
                    "Desc TEXT," +
                    "isFinished BOOL," +
                    "id_task INT," +
                    "FOREIGN KEY(id_task) REFERENCES Tasks(Id)" +
                ")";
            CreateTable(commandString);
        }

        //*******************************************************
        public Color GetCategoryColorById(int id)
        {
            var result = new Color();
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = new SqliteCommand("SELECT Category_color FROM Categorys WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result = Color.FromArgb(reader.GetString(0));
            }
            return result;
        }

        public string GetCategoryNameById(int id)
        {
            var result = "";
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = new SqliteCommand("SELECT Category_name, Id FROM Categorys WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetString(0);
                Console.WriteLine("id = " + reader.GetInt64(1).ToString());
            }
            return result;
        }

        public List<CategoryNote> GetCategorys()
        {
            var result = new List<CategoryNote>();
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "SELECT * FROM Categorys";
            using var command = new SqliteCommand(commandString, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                CategoryNote cat = new();
                cat.ID = reader.GetInt32(0);
                cat.NameCategory = reader.GetString(1);
                cat.Color = Color.FromArgb(reader.GetString(2));

                result.Add(cat);
            }
            return result;
        }

        //*******************************************************
        public void AddNote(NoteItem item)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "INSERT INTO Notes (Note_text, isFavorite, id_category, ModDate) " +
                                "VALUES (@text, @fav, @id_category, @modDate)";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@text", item.NoteText),
                new SqliteParameter("@fav", item.IsFavorite),
                new SqliteParameter("@id_category", (int)item.Category.ID),
                new SqliteParameter("@modDate", item.ModDate)
            });
        }
        public void DeleteNote(NoteItem item)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "DELETE FROM Notes WHERE Id_note = @id";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddWithValue("@id", item.id);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void RewriteNote(NoteItem oldItem, NoteItem newItem)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "UPDATE Notes SET " +
                "Note_text = @text," +
                "isFavorite = @isF," +
                "id_category = @idC," +
                "ModDate = @date " +
                "WHERE Id_note = @id";

            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@text", newItem.NoteText),
                new SqliteParameter("@isF", newItem.IsFavorite),
                new SqliteParameter("@idC", newItem.Category.ID),
                new SqliteParameter("@date", newItem.ModDate),
                new SqliteParameter("@id", oldItem.id)
            });
            command.ExecuteNonQuery();
            connection.Close();
        }

        //*******************************************************
    }
}
