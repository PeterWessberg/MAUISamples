using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace CustomControls.Controls;

public partial class TimeSpanControl : ContentView
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(TimeSpanControl),
        null);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(TimeSpanControl),
        null);

    public static readonly BindableProperty PopupHeaderColorProperty = BindableProperty.Create(
            nameof(PopupHeaderColor),
            typeof(Color),
            typeof(TimeSpanControl),
            Colors.ForestGreen);

    public static readonly BindableProperty DurationProperty = BindableProperty.Create(
            nameof(Duration),
            typeof(TimeSpan),
            typeof(TimeSpanControl),
            TimeSpan.Zero,
            propertyChanged: OnDurationChanged);

    /// <summary>
    /// If True show result immediately on the Label in the view.
    /// </summary>
    public static readonly BindableProperty PreviewProperty = BindableProperty.Create(
            nameof(Preview),
            typeof(bool),
            typeof(TimeSpanControl),
            false);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public Color PopupHeaderColor
    {
        get => (Color)GetValue(PopupHeaderColorProperty);
        set => SetValue(PopupHeaderColorProperty, value);
    }

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public string DurationText
    {
        get => Duration.ToString(@"hh\:mm\:ss");
    }

    public bool Preview
    {
        get => (bool)GetValue(PreviewProperty);
        set => SetValue(PreviewProperty, value);
    }

    private ICommand _executeCommand;
    private TimeSpanPopup popup;

    public ICommand ExecuteCommand =>
        _executeCommand = new Command(async () =>
        {
            Command?.Execute(CommandParameter);
            await OpenPopup();
        });

    public event Action<TimeSpan> DurationChanging;

    public event Action<TimeSpan> DurationChanged;

    private Label timeSpanLabel = new();

    public TimeSpanControl()
    {
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = ExecuteCommand,
            NumberOfTapsRequired = 1
        });

        timeSpanLabel.Text = DurationText;

        Content = new VerticalStackLayout()
        {
            Children =
            {
                timeSpanLabel
            }
        };
    }

    private void OnPopupDurationChanging(TimeSpan span)
    {
        Duration = span;
    }

    private static void OnDurationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TimeSpanControl timeSpanControl)
        {
            timeSpanControl.Duration = (TimeSpan)newValue;
            timeSpanControl.DurationChanging?.Invoke(timeSpanControl.Duration);

            timeSpanControl.timeSpanLabel.Text = timeSpanControl.DurationText;
        }
    }

    private async Task OpenPopup()
    {
        var oldDuration = TimeSpan.Zero;

        popup = new TimeSpanPopup();

        if (Preview)
            popup.DurationChanging += OnPopupDurationChanging;

        popup.Duration = oldDuration = Duration;
        popup.HeaderColor = PopupHeaderColor;

#if DEBUG
        System.Diagnostics.Debug.WriteLine("**************  Opening the Picker **************");
#endif

        var result = await Application.Current.MainPage.ShowPopupAsync(popup);

        if (result is bool boolResult)
        {
            if (boolResult == true)
            {
                Duration = popup.Duration;
                DurationChanged?.Invoke(Duration);
            }
            else
                Duration = oldDuration;
        }
    }
}