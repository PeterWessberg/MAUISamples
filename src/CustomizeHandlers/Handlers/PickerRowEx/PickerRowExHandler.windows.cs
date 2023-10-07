using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using UWPDataTemplate = Microsoft.UI.Xaml.DataTemplate;

namespace CustomizeHandlers.Handlers;

public partial class PickerRowExHandler : PickerHandler
{
    protected override ComboBox CreatePlatformView()
    {
        return new PickerComboBox();
    }

    protected override void ConnectHandler(ComboBox platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(ComboBox platformView)
    {
        base.DisconnectHandler(platformView);
    }
}

public class PickerComboBox : ComboBox
{
    public PickerComboBox()
    {
        // TODO: Proof of concept. Needs further completion but it seem to be the way to go.
        string xaml = "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                      "<TextBlock Text='{Binding}' Foreground='Red'/>" +
                      "</DataTemplate>";

        this.ItemTemplate = (UWPDataTemplate)XamlReader.Load(xaml);
    }
}