using System;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ToDoList.Pages;

public partial class CreateOrEditTaskPage : ContentPage
{
    public static EventHandler<Models.Task> mEventHandler;
    private Models.Task task;

    public CreateOrEditTaskPage()
    {
        InitializeComponent();
        task = new Models.Task { Id = -1, Title = "", Description = "", IsCompleted = false };
    }

    public CreateOrEditTaskPage(Models.Task task)
    {
        InitializeComponent();
        this.task = task;
        EntryTitle.Text = task.Title;
        EditorDescription.Text = task.Description;
    }
    private async void BtnSave_OnClicked(object sender, EventArgs e)
    {
        if (EntryTitle.Text == null || EntryTitle.Text.Length == 0)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await Toast.Make("Please, enter the title of the task.",
                  ToastDuration.Long,
                  16)
            .Show(cancellationTokenSource.Token);

            return;
        }

        EventHandler<Models.Task> handler = mEventHandler;
        if (handler != null)
        {
            this.task.Title = EntryTitle.Text;
            this.task.Description = EditorDescription.Text;
            mEventHandler(this, task);
        }

        await Navigation.PopAsync();
    }
}