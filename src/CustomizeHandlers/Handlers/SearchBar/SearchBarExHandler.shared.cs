using CustomizeHandlers.Controls;
using Microsoft.Maui.Handlers;

namespace CustomizeHandlers.Handlers;

public partial class SearchBarExHandler 
{
    public static readonly IPropertyMapper<ISearchBar, SearchBarHandler> SearchBarMapper =
        new PropertyMapper<ISearchBar, SearchBarHandler>(Mapper)
        {
            //["IconBackgroundColor"] = MapIconBackgroundColor,
            //["IconBackgroundCircle"] = MapIconCircle,
            ["IconColor"] = MapIconColor,
            //["TextBackgroundColor"] = MapTextBackgroundColor,
            ["CancelButtonText"] = MapCancelText,
        };

    public SearchBarExHandler() : base(SearchBarMapper)
    {
    }

    public SearchBarExHandler(IPropertyMapper mapper, CommandMapper commandMapper) : base(mapper ?? SearchBarMapper)
    {
    }

    public override void UpdateValue(string propertyName)
    {
        base.UpdateValue(propertyName);

        if (propertyName == SearchBar.TextColorProperty.PropertyName)
        {
            SetIconColor(GetTextColor());
        }
        if (propertyName == SearchBarLang.CancelButtonTextProperty.PropertyName)
        {
            if (VirtualView is SearchBarLang searchBarLang)
            {
                SetCancelButtonText(searchBarLang.CancelButtonText);
            }
        }
    }

    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler searchBarHandler)
            searchBarHandler.SetIconColor(searchBarHandler.GetTextColor());
    }

    private static void MapCancelText(SearchBarHandler handler, ISearchBar bar)
    {
        if (handler is SearchBarExHandler searchBarHandler && searchBarHandler.VirtualView is SearchBarLang searchBarLang)
        {
            searchBarHandler.SetCancelButtonText(searchBarLang.CancelButtonText);
        }
    }

}