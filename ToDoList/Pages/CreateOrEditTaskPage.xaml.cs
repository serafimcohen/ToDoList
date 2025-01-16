using ToDoList.PageModels;

namespace ToDoList.Pages;

public partial class CreateOrEditTaskPage : ContentPage
{
    public CreateOrEditTaskPage()
    {
        BindingContext = new CreateOrEditTaskPageModel();
        InitializeComponent();
    }
}