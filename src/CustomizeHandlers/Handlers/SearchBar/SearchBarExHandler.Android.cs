using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AColor = Android.Graphics.Color;

namespace CustomizeHandlers.Handlers;

public partial class SearchBarExHandler : SearchBarHandler
{
    public void SetIconColor(AColor value)
    {
        var searchIcon = (ImageView)PlatformView.FindViewById(Resource.Id.search_mag_icon);
        searchIcon.SetColorFilter(value, PorterDuff.Mode.SrcAtop);
    }

    public AColor GetTextColor() => VirtualView.TextColor.ToPlatform();

    public void SetCancelButtonText(string newCancelButtonText)
    {
    }

    //TODO
    public static void MapIconBackgroundColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            var searchView = customHandler.PlatformView;
            searchView.FindViewById(Resource.Id.search_mag_icon).SetBackgroundColor(Android.Graphics.Color.LightGray);
        }
    }

    public static void MapTextBackgroundColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            var searchView = customHandler.PlatformView;
            var searchEditText = (EditText)searchView.FindViewById(Resource.Id.search_src_text);
            searchEditText.SetBackgroundColor(Colors.LightGray.ToAndroid());
        }
    }

    public static void MapIconCircle(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            var searchView = customHandler.PlatformView;
            var searchIcon = (ImageView)searchView.FindViewById(Resource.Id.search_mag_icon);
            searchIcon.SetBackgroundResource(Resource.Drawable.circle_border);
            searchIcon.SetPadding(0, 10, 0, 10);
        }
    }
}