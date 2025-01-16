using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ToDoList.Utils
{
    public class UIUtils
    {
        public static async void showToastMessageAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await Toast.Make(message,
                  ToastDuration.Long,
                  16)
            .Show(cancellationTokenSource.Token);
        }
    }
}
