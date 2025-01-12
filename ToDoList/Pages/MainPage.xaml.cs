using System.Collections.ObjectModel;
using ToDoList.Data;

namespace ToDoList.Pages

{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Models.Task> tasks = new ObservableCollection<Models.Task>();
        public ObservableCollection<Models.Task> Tasks => tasks;
        private DatabaseHandler databaseHandler;

        public MainPage()
        {
            InitializeComponent();

            databaseHandler = new DatabaseHandler();

            CreateOrEditTaskPage.mEventHandler += delegate (object? obj, Models.Task task)
            {
                BackCall(obj, task);
            };

            LoadTasksAsync();

            tasksCollectionView.ItemsSource = tasks;
        }

        private async void LoadTasksAsync()
        {
            List<Models.Task> list = await databaseHandler.GetItemsAsync();
            list.ForEach(tasks.Add);
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
                    task.Id = await databaseHandler.SaveItemAsync(task);
                    tasks.Add(task);
                }
                else
                {
                    await databaseHandler.UpdateItemTitleAndDescriptionAsync(task);
                }
            }
        }

        private void BtnAddTask_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateOrEditTaskPage());
        }

        private async void BtnEdit_OnClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;

            if (button.BindingContext is not Models.Task task)
                return;

            var createOrEditTaskPage = new CreateOrEditTaskPage(task);
            await Navigation.PushAsync(createOrEditTaskPage);
        }

        private async void BtnDelete_OnClicked(object sender, EventArgs e)
        {
            var button = (ImageButton) sender;

            if (button.BindingContext is not Models.Task task)
                return;

            tasks.Remove(task);

            await databaseHandler.DeleteItemAsync(task);
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;

            if (checkbox.BindingContext is not Models.Task task)
                return;

            task.IsCompleted = e.Value;

            await databaseHandler.UpdateItemIsCompletedAsync(task);
        }
    }

}
