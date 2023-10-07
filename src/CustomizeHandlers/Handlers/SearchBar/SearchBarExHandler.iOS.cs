using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace CustomizeHandlers.Handlers;

public partial class SearchBarExHandler : SearchBarHandler
{
    public void SetIconColor(UIColor value)
    {
#if IOS13_0_OR_GREATER
        var textField = PlatformView.SearchTextField;
        var leftView = textField.LeftView ?? throw new Exception();
        leftView.TintColor = value;
#endif
    }

    private UIColor GetTextColor() => VirtualView.TextColor.ToPlatform();
}