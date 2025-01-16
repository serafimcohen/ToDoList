using SQLite;

namespace ToDoList.Data
{
    public class DatabaseHandler
    {
        private SQLiteAsyncConnection Database;

        public DatabaseHandler()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TITLE TEXT NOT NULL,
                    DESCRIPTION TEXT NOT NULL,
                    ISCOMPLETED BOOLEAN NOT NULL
                );");
        }
        public async Task<long> SaveItemAsync(Models.Task task)
        {
            await Init();

            await Database.ExecuteAsync(
                $@"INSERT INTO Tasks (TITLE, DESCRIPTION, ISCOMPLETED)
                    VALUES(""{task.Title}"", ""{task.Description}"", {task.IsCompleted});");

            return await Database.ExecuteScalarAsync<long>(@"select last_insert_rowid()");
        }

        public async Task<long> UpdateItemTitleAndDescriptionAsync(Models.Task task)
        {
            await Init();

            await Database.ExecuteAsync(
                $@"UPDATE Tasks SET 
                    TITLE = ""{task.Title}"", 
                    DESCRIPTION = ""{task.Description}""
                    WHERE ID = {task.Id};");

            return 0;
        }

        public async Task<long> UpdateItemIsCompletedAsync(Models.Task task)
        {
            await Init();

            await Database.ExecuteAsync(
                $@"UPDATE Tasks SET 
                    ISCOMPLETED = {task.IsCompleted} 
                    WHERE ID = {task.Id};"); 

            return 0;
        }

        public async Task<List<Models.Task>> GetItemsAsync()
        {
            await Init();

            return await Database.QueryAsync<Models.Task>(@"SELECT * FROM Tasks");
        }

        public async Task<int> DeleteItemAsync(Models.Task task)
        {
            await Init();

            await Database.ExecuteAsync($@"
                DELETE FROM Tasks WHERE ID = {task.Id};");

            return 0;
        }
    }
}
