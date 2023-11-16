using Foundation;
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

    public void SetCancelButtonText(string newCancelButtonText)
    {
        PlatformView.SetValueForKey(NSObject.FromObject(newCancelButtonText), new NSString("cancelButtonText"));

        // Now we need to activate the button so it refreshes and take our changes.
        // If the searchbar is empty, this happens automatic when you start to type. 
        if (PlatformView is UISearchBar uiSearchBar)
        {
            bool hasText = !string.IsNullOrEmpty(uiSearchBar.Text);

            if(VirtualView.CancelButtonColor is not null)
            {
                 // Honor the text color. 
                var cancelButtonAttributes = new UIStringAttributes
                {
                    ForegroundColor = VirtualView.CancelButtonColor.ToPlatform()
                };
                UIBarButtonItem.AppearanceWhenContainedIn(typeof(UISearchBar)).SetTitleTextAttributes(cancelButtonAttributes, UIControlState.Normal);
            }

            uiSearchBar.ShowsCancelButton = false;
            if (hasText)
            {
                uiSearchBar.ShowsCancelButton = true;
            }

            if (uiSearchBar.ValueForKey(new NSString("cancelButton")) is UIButton cancelButton)
            {
                cancelButton.Enabled = hasText;
            }
        }
    }

    public static void MapTextBackgroundColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            if(customHandler.PlatformView is UISearchBar uiSearchBar) 
            {
                uiSearchBar.BarTintColor = customHandler.VirtualView.CancelButtonColor.ToPlatform();
            }
        }
    }
}