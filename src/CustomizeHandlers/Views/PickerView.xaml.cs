namespace CustomizeHandlers.Views;

public partial class PickerView : ContentPage
{
	public PickerView()
	{
		InitializeComponent();
	}

    private void MyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var color = (string)picker.Items[selectedIndex];
            PickerLabel.TextColor = ColorFromString(color);
            PickerLabel.Text = $"Picked item was: {color}";
            MyPicker.TextColor = ColorFromString(color);
        }
    }

    private void PickerView_Unloaded(object sender, EventArgs e)
    {
        MyPicker.Handler?.DisconnectHandler();
    }

    public static Color ColorFromString(string colorName)
    {
        return colorName.ToLower() switch
        {
            "green" => Colors.Green,
            "purple" => Colors.Purple,
            "blue" => Colors.Blue,
            "yellow" => Colors.Yellow,
            "magenta" => Colors.Magenta,
            _ => Colors.Black 
        };
    }
}