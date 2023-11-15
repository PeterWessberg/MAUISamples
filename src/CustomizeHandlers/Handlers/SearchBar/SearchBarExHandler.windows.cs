using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using WColor = Windows.UI.Color;

namespace CustomizeHandlers.Handlers;

public partial class SearchBarExHandler : SearchBarHandler
{
    public void SetIconColor(WColor value)
    {
        PlatformView.QueryIcon.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(value);
    }

    private WColor GetTextColor() => VirtualView.TextColor.ToWindowsColor();

    public void SetCancelButtonText(string newCancelButtonText)
    {
    }
}