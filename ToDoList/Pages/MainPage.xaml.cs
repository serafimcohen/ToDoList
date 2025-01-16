using ToDoList.PageModels;

namespace ToDoList.Pages

{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            BindingContext = new MainPageModel();
            InitializeComponent();
        }
    }
}
