using System.Globalization;

namespace CustomizeHandlers.Controls;

public class RadioButtonGroupLang : StackLayout
{
    public RadioButton RadioButton1 { get; private set; }
    public RadioButton RadioButton2 { get; private set; }

    public event EventHandler<string> LanguageChanged;

    public RadioButtonGroupLang()
    {
        Orientation = StackOrientation.Horizontal;
        RadioButton1 = new RadioButton { Content = "English", Padding = new Thickness(10, 0, 0, 0) };
        RadioButton2 = new RadioButton { Content = "Swedish", Padding = new Thickness(10, 0, 0, 0) };

        RadioButton1.CheckedChanged += OnRadioButtonCheckedChanged;
        RadioButton2.CheckedChanged += OnRadioButtonCheckedChanged;

        Children.Add(RadioButton1);
        Children.Add(RadioButton2);

        RadioButton1.IsChecked = true;
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var radioButton = (RadioButton)sender;
        string languageCode = "";

        if (radioButton == RadioButton1 && RadioButton1.IsChecked)
        {
            RadioButton2.IsChecked = false;
            languageCode = "en";
        }
        else if (radioButton == RadioButton2 && RadioButton2.IsChecked)
        {
            languageCode = "sv";
        }

        if (!string.IsNullOrEmpty(languageCode))
        {
            CultureInfo newCulture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;

            LanguageChanged?.Invoke(this, languageCode);
        }
    }
}
