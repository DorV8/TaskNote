
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

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
        //*******************************************************

        public void ClearData()
        {
            List<string> names = [
                "Notes","Stages", "Tasks"
            ];
            foreach (string name in names)
            {
                try
                {
                    ClearTable(name);
                }
                catch
                {
                    Console.WriteLine("ошибка при очищении таблицы");
                }
            }
        }
        private void ClearTable(string tableName) 
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var commandString = "DELETE FROM " + tableName;
            using var command = new SqliteCommand(commandString, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        //*******************************************************
        //Creating tables
        private void CreateTable(string commandString)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            using var command = new SqliteCommand(commandString, connection);
            try
            {
                var test = command.ExecuteNonQuery();
                Console.WriteLine(test);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            connection.Close();
        }
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
        public void CreateTasks()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS Tasks " +
                "(" +
                    "Id INTEGER PRIMARY KEY," +
                    "ModDate DATE," +
                    "Header TEXT," +
                    "Desc TEXT," +
                    "IsFavorite BOOLEAN," +
                    "IsAlarmed BOOLEAN," +
                    "AlarmDate DATE" +
               ")";
            CreateTable(commandString);
        }
        public void CreateStages()
        {
            var commandString = "CREATE TABLE IF NOT EXISTS Stages" +
                "(" +
                    "Id INTEGER PRIMARY KEY," +
                    "Header TEXT," +
                    "Desc TEXT," +
                    "IsCompleted BOOLEAN," +
                    "id_task INT," +
                    "FOREIGN KEY(id_task) REFERENCES Tasks(Id)" +
                ")";
            CreateTable(commandString);
        }

        //*******************************************************
        //categorys commands
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
        //notes commands
        public ObservableCollection<NoteItem> GetNotes()
        {
            ObservableCollection<NoteItem> result = [];
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
                result.Add(note);
            }
            connection.Close();

            return result;
        }
        public void AddNote(NoteItem item)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "INSERT INTO Notes (Note_text, isFavorite, id_category, ModDate) " +
                                "VALUES (@text, @fav, @id_category, @modDate)";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(
            [
                new SqliteParameter("@text", item.NoteText),
                new SqliteParameter("@fav", item.IsFavorite),
                new SqliteParameter("@id_category", (int)item.Category.ID),
                new SqliteParameter("@modDate", item.ModDate)
            ]);
            command.ExecuteNonQuery();
            connection.Close();
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
        //tasks commands
        public ObservableCollection<TaskItem> GetTasks()
        {
            var result = new ObservableCollection<TaskItem>();
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = new SqliteCommand("SELECT * FROM Tasks", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = new TaskItem()
                {
                    id = Int32.Parse(reader.GetInt64(0).ToString()),
                    ModDate = reader.GetDateTime(1),
                    TaskHeader = reader.GetString(2),
                    TaskDesc = reader.GetString(3),
                    IsFavorite = reader.GetBoolean(4),
                    IsAlarmed = reader.GetBoolean(5),
                    AlarmDate = reader.GetDateTime(6)
                };
                item.AllStages = GetStages(item.id);
                result.Add(item);
            }
            connection.Close();
            return result;
        }

        public void AddTask(TaskItem item)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "INSERT INTO Tasks(ModDate, Header, Desc, IsFavorite, IsAlarmed, AlarmDate) " +
                                "VALUES(@modDate, @header, @desc, @fav, @isAlarmed, @AlarmDate)";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@modDate", item.ModDate),
                new SqliteParameter("@header", item.TaskHeader),
                new SqliteParameter("@desc", item.TaskDesc),
                new SqliteParameter("@fav", item.IsFavorite),
                new SqliteParameter("@isAlarmed", item.IsAlarmed),
                new SqliteParameter("@AlarmDate", item.AlarmDate)
            });
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void RemoveTask(TaskItem item)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            foreach(var stage in item.AllStages)
            {
                RemoveStage(stage.id);
            }
            using var command = new SqliteCommand("DELETE FROM Tasks WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", item.id);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void RewriteTask(int oldItemId, TaskItem newItem)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var commandString = "UPDATE Tasks SET " +
                                "ModDate = @modDate," +
                                "Header = @head," +
                                "Desc = @desc," +
                                "IsFavorite = @fav," +
                                "IsAlarmed = @alarm," +
                                "AlarmDate = @alarmDate " +
                                "WHERE Id = @id";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@id", oldItemId),
                new SqliteParameter("@modDate", newItem.ModDate),
                new SqliteParameter("@head",newItem.TaskHeader),
                new SqliteParameter("@desc", newItem.TaskDesc),
                new SqliteParameter("fav", newItem.IsFavorite),
                new SqliteParameter("@alarm",newItem.IsAlarmed),
                new SqliteParameter("@alarmDate", newItem.AlarmDate)
            });
            command.ExecuteNonQuery();
            foreach(var stage in newItem.AllStages)
            {
                RewriteStage(stage.id, stage, oldItemId);
            }
            connection.Close();
        }
        //*******************************************************
        //stages commands
        public ObservableCollection<TaskStageItem> GetStages(int id_task)
        {
            var result = new ObservableCollection<TaskStageItem>();

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "SELECT * FROM Stages WHERE id_task = @id";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddWithValue("@id", id_task);
            using var commandCount = new SqliteCommand("SELECT count(Id) FROM Stages", connection);
            var count = Convert.ToInt64(commandCount.ExecuteScalar());
            if (count > 0)
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new TaskStageItem()
                    {
                        id = Int32.Parse(reader.GetInt64(0).ToString()),
                        TaskStageHeader = reader.GetString(1),
                        TaskStageDesc = reader.GetString(2),
                        IsCompleted = reader.GetBoolean(3)
                    });
                }
            }            
            connection.Close();
            return result;
        }

        public async void AddStage(TaskStageItem item, int id_task)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "INSERT INTO Stages(Header, Desc, IsCompleted, id_task) " +
                                "VALUES(@head, @desc, @finish, @id_task)";
            using var command = new SqliteCommand( commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@head", item.TaskStageHeader),
                new SqliteParameter("@desc", item.TaskStageDesc),
                new SqliteParameter("@finish", item.IsCompleted),
                new SqliteParameter("@id_task", id_task)
            });
            var test = await command.ExecuteNonQueryAsync();
            Console.WriteLine("Количество затронутых строк: " + test);
            connection.Close();
        }

        public void RemoveStage(int itemId)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = new SqliteCommand("DELETE FROM Stages WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", itemId);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void RewriteStage(int oldItemId, TaskStageItem newItem, int id_task)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var commandString = "UPDATE Stages SET " +
                                "Header = @head," +
                                "Desc = @desc," +
                                "IsCompleted = @completed," +
                                "id_task = @id_task " +
                                "WHERE id = @id";
            using var command = new SqliteCommand(commandString, connection);
            command.Parameters.AddRange(new[]
            {
                new SqliteParameter("@id", oldItemId),
                new SqliteParameter("@head", newItem.TaskStageHeader),
                new SqliteParameter("@desc", newItem.TaskStageDesc),
                new SqliteParameter("@completed", newItem.IsCompleted),
                new SqliteParameter("id_task", id_task)
            });
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
