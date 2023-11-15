using CustomizeHandlers.Resources.Localization;

namespace CustomizeHandlers.Views;

public partial class SearcBarView : ContentPage
{
    private readonly Color[] colors;
    private int timesPressedButton = 0;

    public SearcBarView()
    {
        InitializeComponent();
        RadioButtons.LanguageChanged += RadioButtons_LanguageChanged;

        colors = new Color[]
        {
            Colors.Green,
            Colors.Purple,
            Colors.Blue,
            Colors.Yellow,
            Colors.Magenta,
        };
    }

    private void RadioButtons_LanguageChanged(object sender, string e)
    {
        MySearchBar.CancelButtonText = AppResources.CancelButtonText;
    }

    private void SearcBarView_Unloaded(object sender, EventArgs e)
    {
        MySearchBar.Handler?.DisconnectHandler();
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        string searchText = searchBar.Text;
    }

    private void OnToggleThemeClicked(object sender, EventArgs e)
    {
        AppTheme currentTheme = Application.Current.RequestedTheme;
        if (Application.Current.RequestedTheme == AppTheme.Unspecified)
        {
        }
        if (Application.Current.UserAppTheme == AppTheme.Unspecified)
            Application.Current.UserAppTheme = Application.Current.RequestedTheme;

        if (Application.Current.UserAppTheme == AppTheme.Dark)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
    }

    private void TextColor_Clicked(object sender, EventArgs e)
    {
        var colorIndex = timesPressedButton % 5;
        var color = colors[colorIndex];
        MySearchBar.TextColor = color;

        timesPressedButton++;
    }
}