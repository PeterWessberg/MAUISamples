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
    }

    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler searchBarHandler)
            searchBarHandler.SetIconColor(searchBarHandler.GetTextColor());
    }
}