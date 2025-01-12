using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using SQLite;

namespace ToDoList.Data
{
    public class DatabaseHandler
    {
        private SQLiteAsyncConnection Database;

        public DatabaseHandler()
        {
        }

        private async void showToastError(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await Toast.Make("Error: " + message,
                  ToastDuration.Long,
                  16)
            .Show(cancellationTokenSource.Token);
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            try
            {
                await Database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TITLE TEXT NOT NULL,
                    DESCRIPTION TEXT NOT NULL,
                    ISCOMPLETED BOOLEAN NOT NULL
                );");
            }
            catch (Exception e)
            {
                showToastError(e.Message);
            }
        }
        public async Task<long> SaveItemAsync(Models.Task task)
        {
            await Init();

            try
            {
                await Database.ExecuteAsync(@"
                INSERT INTO Tasks (TITLE, DESCRIPTION, ISCOMPLETED)
                    VALUES("
                    + "\"" + task.Title + "\", "
                    + "\"" + task.Description + "\", "
                    + task.IsCompleted + ");");

                return await Database.ExecuteScalarAsync<long>(@"select last_insert_rowid()");
            }
            catch (Exception e)
            {
                showToastError(e.Message);

                return -1;
            }
        }

        public async Task<long> UpdateItemTitleAndDescriptionAsync(Models.Task task)
        {
            await Init();

            try
            {
                await Database.ExecuteAsync(@"
                UPDATE Tasks
                    SET TITLE = " + "\"" + task.Title + "\","
                    + " DESCRIPTION = " + "\"" + task.Description + "\""
                    + " WHERE ID = " + task.Id + ";");

                return 0;
            }
            catch (Exception e)
            {
                showToastError(e.Message);

                return -1;
            }
        }

        public async Task<long> UpdateItemIsCompletedAsync(Models.Task task)
        {
            await Init();

            try
            {
                await Database.ExecuteAsync(@"
                UPDATE Tasks
                    SET ISCOMPLETED = " + task.IsCompleted
                    + " WHERE ID = " + task.Id + ";");

                return 0;
            }
            catch (Exception e)
            {
                showToastError(e.Message);

                return -1;
            }
        }

        public async Task<List<Models.Task>> GetItemsAsync()
        {
            await Init();

            try
            {
                return await Database.QueryAsync<Models.Task>(@"SELECT * FROM Tasks");
            }
            catch (Exception e)
            {
                showToastError(e.Message);

                return null;
            }
        }

        public async Task<int> DeleteItemAsync(Models.Task task)
        {
            await Init();

            try
            {
                await Database.ExecuteAsync(@"
                DELETE FROM Tasks
                    WHERE ID = " + task.Id + ";");

                return 0;
            }
            catch (Exception e)
            {
                showToastError(e.Message);

                return -1;
            }
        }
    }
}
