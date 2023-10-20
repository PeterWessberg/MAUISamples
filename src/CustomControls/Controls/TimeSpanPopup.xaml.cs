using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace CustomControls.Controls;

public partial class TimeSpanPopup : Popup
{
    public static readonly BindableProperty DurationProperty = BindableProperty.Create(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(TimeSpanPopup),
        TimeSpan.Zero,
        BindingMode.TwoWay,
        propertyChanged: OnDurationChanged);

    public static readonly BindableProperty HeaderColorProperty = BindableProperty.Create(
        nameof(HeaderColor),
        typeof(Color),
        typeof(TimeSpanPopup),
        Colors.ForestGreen);

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set
        {
            SetValue(DurationProperty, value);
            SetTime();
        }
    }

    public Color HeaderColor
    {
        get => (Color)GetValue(HeaderColorProperty);
        set
        {
            SetValue(HeaderColorProperty, value);
            HeaderBackground.Color = value;
        }
    }

    private Button currentDigitPair;

    private TimeSpan _oldDuration = TimeSpan.Zero;

    private int currentPosition;

    public event Action<TimeSpan> DurationChanging;

    public TimeSpanPopup()
    {
        InitializeComponent();
        var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
        Size = new Size(Math.Min(300, mainDisplayInfo.Width / mainDisplayInfo.Density), Math.Min(345, mainDisplayInfo.Height / mainDisplayInfo.Density));

        currentDigitPair = SecondButton;
        currentPosition = 2;
        SetDigitPairBorders(currentDigitPair, true);
    }

    private static void OnDurationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var popup = bindable as TimeSpanPopup;
        popup.DurationChanging?.Invoke(popup.Duration);

        // Lets remeber the old value if they press cancel
        if (popup._oldDuration == TimeSpan.Zero)
            popup._oldDuration = popup.Duration;
    }

    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        PutDigit("0");
    }

    private void OnDigitClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        PutDigit(button.Text);
    }

    private void SetTime()
    {
        HourButton.Text = Duration.Hours.ToString("D2");
        MinuteButton.Text = Duration.Minutes.ToString("D2");
        SecondButton.Text = Duration.Seconds.ToString("D2");
    }

    private void OnTimeButtonsClicked(object? sender, EventArgs e)
    {
        SetDigitPairBorders(sender, true);
        currentDigitPair = (Button)sender;
        currentPosition = 2;
    }

    private void SetDigitPairBorders(object sender, bool on = false)
    {
        SetButtonBorders(HourButton);
        SetButtonBorders(MinuteButton);
        SetButtonBorders(SecondButton);

        SetButtonBorders(sender, true);
    }

    public static void SetButtonBorders(object sender, bool on = false)
    {
        var button = (Button)sender;
        if (on)
        {
            button.BorderColor = Colors.DarkGray;
            button.BorderWidth = 1;
        }
        else
        {
            button.BorderWidth = 0;
        }
    }

    private void PutDigit(string digit)
    {
        var value = currentDigitPair.Text.Remove(currentPosition - 1, 1).Insert(currentPosition - 1, digit);
        if (currentDigitPair == HourButton && !IsValidHour(value)) return;
        if (!IsValidMinuteOrSecond(value)) return;

        currentDigitPair.Text = value;
        MoveNextPosition();

        Duration = new TimeSpan(int.Parse(HourButton.Text), int.Parse(MinuteButton.Text), int.Parse(SecondButton.Text));
    }

    private void MoveNextPosition()
    {
        currentPosition--;
        if (currentPosition == 0)
        {
            currentPosition = 2;
            if (currentDigitPair == SecondButton)
            {
                currentDigitPair = MinuteButton;
            }
            else
            if (currentDigitPair == MinuteButton)
            {
                currentDigitPair = HourButton;
            }
            else currentPosition = 1;
        }

        SetDigitPairBorders(currentDigitPair, true);
    }

    private void OnDoubleZeroClicked(object? sender, EventArgs args)
    {
        currentDigitPair.Text = "00";
        currentPosition = 2;

        Duration = new TimeSpan(int.Parse(HourButton.Text), int.Parse(MinuteButton.Text), int.Parse(SecondButton.Text));

        //MoveNextPosition();
        //if(currentDigitPair == HourButton) currentPosition = 2;
    }

    private void OnOKButtonClicked(object? sender, EventArgs e)
    {
        Duration = new TimeSpan(int.Parse(HourButton.Text), int.Parse(MinuteButton.Text), int.Parse(SecondButton.Text));
        Close(true);
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e) => Close(false);

    public static bool IsValidHour(string hourStr)
    {
        if (int.TryParse(hourStr, out int hour))
        {
            return hour >= 0 && hour <= 23;
        }
        return false;
    }

    public static bool IsValidMinuteOrSecond(string minuteOrSecondStr)
    {
        if (int.TryParse(minuteOrSecondStr, out int minuteOrSecond))
        {
            return minuteOrSecond >= 0 && minuteOrSecond <= 59;
        }
        return false;
    }
}