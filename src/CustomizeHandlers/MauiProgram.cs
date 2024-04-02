using Microsoft.Extensions.Logging;
using CustomizeHandlers.Controls;
using CustomizeHandlers.Handlers;

namespace CustomizeHandlers;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<PickerRowEx, PickerRowExHandler>();
                handlers.AddHandler<SearchBar, SearchBarExHandler>();
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}