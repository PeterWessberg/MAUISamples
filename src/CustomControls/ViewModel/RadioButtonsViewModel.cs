using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using CustomControls.Controls;

namespace CustomControls.ViewModel;

public partial class RadioButtonsViewModel
{
    public List<ControlButton> MyButtons { get; } = new List<ControlButton>()
    {
        new ControlButton() { LabelText = "Test 1", DefaultColor = Colors.Blue, CheckedColor = Colors.Blue },
        new ControlButton() { LabelText = "Test 2", IsChecked = true, DefaultColor = Colors.Green,  CheckedColor = Colors.Green },
        new ControlButton() { LabelText = "Test 3", DefaultColor = Colors.Purple,  CheckedColor = Colors.Purple},
        new ControlButton() { LabelText = "Test 4", DefaultColor = Colors.Orange,  CheckedColor = Colors.Orange}
    };

    public RadioButtonsViewModel()
    {
    }

    [RelayCommand]
    private async Task ButtonSelected(RadioButtonItem button)
    {
        await Toast.Make($"Button pressed (VM): {button.Text} ").Show();
    }
}