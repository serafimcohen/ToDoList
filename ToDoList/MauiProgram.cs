using CommunityToolkit.Maui;
using Fonts;
using Microsoft.Extensions.Logging;
using ToDoList.PageModels;

namespace ToDoList
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<Data.DatabaseHandler>();
            builder.Services.AddSingleton<MainPageModel>();
            builder.Services.AddSingleton<CreateOrEditTaskPageModel>();

            return builder.Build();
        }
    }
}
