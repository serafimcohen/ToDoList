using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoList.Data;
using ToDoList.Utils;

namespace ToDoList.PageModels
{
    public partial class MainPageModel
    {
        private ObservableCollection<Models.Task> tasks;
        public ObservableCollection<Models.Task> Tasks => tasks;
        private readonly DatabaseHandler databaseHandler;

        public ICommand OpenCreateTaskPage { get; }
        public ICommand OpenEditTaskPage { get; }
        public ICommand DeleteTask { get; }
        public ICommand UpdateTaskIsCompleted { get; }

        public MainPageModel()
        {
            databaseHandler = new DatabaseHandler();
            tasks = new ObservableCollection<Models.Task>();
            OpenCreateTaskPage = new Command(ExecuteOpenCreateTaskPage);
            OpenEditTaskPage = new Command<Models.Task>(ExecuteOpenEditTaskPage);
            DeleteTask = new Command<Models.Task>(ExecuteDeleteTask);
            UpdateTaskIsCompleted = new Command<Models.Task>(ExecuteUpdateTaskIsCompleted);
            LoadTasksAsync();

            CreateOrEditTaskPageModel.mEventHandler += delegate (object? obj, Models.Task task)
            {
                BackCall(obj, task);
            };
        }

        private async void LoadTasksAsync()
        {
            try
            {
                List<Models.Task> list = await databaseHandler.GetItemsAsync();
                list.ForEach(tasks.Add);
            }
            catch (Exception)
            {
                UIUtils.showToastMessageAsync($"Error while querying items from database.");
            }
        }

        private async void BackCall(object? obj, Models.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            else
            {
                if (task.Id == -1)
                {
                    try
                    {
                        task.Id = await databaseHandler.SaveItemAsync(task);
                        tasks.Add(task);
                    }
                    catch (Exception)
                    {
                        UIUtils.showToastMessageAsync($"Error while inserting item {task.Id} to database.");
                    }
                }
                else
                {
                    try
                    {
                        await databaseHandler.UpdateItemTitleAndDescriptionAsync(task);
                    }
                    catch (Exception)
                    {
                        UIUtils.showToastMessageAsync($"Error while updating item {task.Id} title and description in database.");
                    }
                }
            }
        }

        private void ExecuteOpenCreateTaskPage()
        {
            var navigationParameters = new Dictionary<string, object>
            {
                {"task", null},
            };
            Shell.Current.GoToAsync($"CreateOrEditTaskPage", false, navigationParameters);
        }

        private void ExecuteOpenEditTaskPage(Models.Task task)
        {
            if (task == null)
                return;

            var navigationParameters = new Dictionary<string, object>
            {
                {"task", task},
            };
            Shell.Current.GoToAsync($"CreateOrEditTaskPage", false, navigationParameters);
        }

        private async void ExecuteDeleteTask(Models.Task task)
        {
            if (task == null)
                return;

            try
            {
                await databaseHandler.DeleteItemAsync(task);
                tasks.Remove(task);
            }
            catch (Exception e)
            {
                UIUtils.showToastMessageAsync($"Error while removing item {task.Id} from database.");
            }
        }

        private async void ExecuteUpdateTaskIsCompleted(Models.Task task)
        {
            if (task == null)
                return;

            try
            {
                await databaseHandler.UpdateItemIsCompletedAsync(task);
            }
            catch (Exception)
            {
                UIUtils.showToastMessageAsync($"Error while updating item {task.Id} is completed in database.");
            }
        }
    }
}
