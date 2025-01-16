using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Utils;

namespace ToDoList.PageModels
{
    public partial class CreateOrEditTaskPageModel : ObservableObject, IQueryAttributable
    {
        public static EventHandler<Models.Task> mEventHandler;
        public ICommand SaveTask { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private Models.Task task;

        [ObservableProperty]
        public string title = string.Empty;
        [ObservableProperty]
        public string description = string.Empty;

        public CreateOrEditTaskPageModel()
        {
            SaveTask = new Command(ExecuteSaveTask);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            object obj = query["task"];
            if (obj == null)
            {
                task = new Models.Task { Id = -1, Title = "", Description = "", IsCompleted = false };
            }
            else
            {
                task = obj as Models.Task;
            }

            Title = task.Title;
            Description = task.Description;
        }

        private async void ExecuteSaveTask()
        {
            if (Title == null || Title.Length == 0)
            {
                UIUtils.showToastMessageAsync("Please, enter the title of the task.");
                return;
            }

            await Shell.Current.GoToAsync($"..");

            EventHandler<Models.Task> handler = mEventHandler;
            if (handler != null)
            {
                task.Title = Title;
                task.Description = Description;
                mEventHandler(this, task);
            }
        }
    }
}
