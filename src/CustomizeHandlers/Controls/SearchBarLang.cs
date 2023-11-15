using CustomizeHandlers.Handlers;
using CustomizeHandlers.Resources.Localization;

namespace CustomizeHandlers.Controls;

public partial class SearchBarLang : SearchBar
{
    public static readonly BindableProperty CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(SearchBarLang), defaultValue: AppResources.CancelButtonText);

    public string CancelButtonText
    {
        get => (string)GetValue(CancelButtonTextProperty);
        set => SetValue(CancelButtonTextProperty, value);
    }
}
