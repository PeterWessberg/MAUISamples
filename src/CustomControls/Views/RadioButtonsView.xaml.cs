using CommunityToolkit.Maui.Alerts;
using CustomControls.Controls;
using CustomControls.ViewModel;

namespace CustomControls.Views;

public partial class RadioButtonsView : ContentPage
{
    public RadioButtonsView()
    {
        InitializeComponent();
        BindingContext = new RadioButtonsViewModel();
        MyRadioButtonsControl.CheckedChanged += OnControlChanged;
    }

    private void OnControlChanged(object sender, CheckedChangedEventArgs e)
    {
        var label = (RadioButtonItem)sender;
        Toast.Make($"Button pressed (View): {label.Text}").Show();
    }
}