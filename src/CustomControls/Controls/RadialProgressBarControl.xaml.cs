using CustomControls.Drawables;

namespace CustomControls.Controls;

public partial class RadialProgressBarControl : ContentView
{
    public static readonly BindableProperty BarThicknessProperty = BindableProperty.Create(
                        nameof(BarThickness),
                        typeof(double),
                        typeof(RadialProgressBarControl),
                        defaultValue: 10d,
                        propertyChanged: Invalidate);

    public double BarThickness
    {
        get { return (double)GetValue(BarThicknessProperty); }
        set { SetValue(BarThicknessProperty, value); }
    }

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
                    nameof(Progress),
                    typeof(double),
                    typeof(RadialProgressBarControl),
                    defaultValue: 100d,
                    BindingMode.TwoWay,
                    propertyChanged: Invalidate);

    public double Progress
    {
        get
        {
            return (double)GetValue(ProgressProperty);
        }
        set
        {
            SetValue(ProgressProperty, value);
            OnPropertyChanged(nameof(this.Progress));
        }
    }

    public static readonly BindableProperty TextColorProperty =
    BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(RadialProgressBarControl),
        defaultValue: Colors.Black,
        propertyChanged: OnTextColorChange);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(RadialProgressBarControl),
        defaultValue: 12d,
        propertyChanged: OnFontSizeChanged);

    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty BarBackgroundColorProperty =
    BindableProperty.Create(
        nameof(BarBackgroundColor),
        typeof(Color),
        typeof(RadialProgressBarControl),
        defaultValue: null,
        propertyChanged: Invalidate);

    public Color BarBackgroundColor
    {
        get { return (Color)GetValue(BarBackgroundColorProperty); }
        set { SetValue(BarBackgroundColorProperty, value); }
    }

    private readonly RadialProgressbarDrawable _RadialProgressbarDrawable;

    public RadialProgressBarControl()
    {
        InitializeComponent();

        _RadialProgressbarDrawable = new RadialProgressbarDrawable(this);

        Initialize();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        var ratio = Math.Min(Width, Height);
        ProgressView.HeightRequest = ratio;
        ProgressView.WidthRequest = ratio;
        var fontSize = ratio * 0.3;
        ProgressLabel.FontSize = FontSize > fontSize ? fontSize : FontSize;
        UpdateProgressNumber();
        ProgressView.Invalidate();
    }

    public void Initialize()
    {
        ProgressView.Drawable = _RadialProgressbarDrawable;
    }

    private void UpdateProgressNumber()
    {
        string num;

        // First make sure we don't have to deal with larger than 100% values
        var remainder = Progress > 100 ? Progress % 100 : Progress;
        Progress = Math.Round(remainder, 1);

        // Next round it off with one digit
        if (Progress % 1 == 0) // Check if the number is a whole number
        {
            num = string.Format("{0:0}", (float)Progress);
        }
        else
        {
            num = string.Format("{0:0.0}", (float)Progress);
        }

        ProgressLabel.Text = $"{num}%";
    }

    private static void OnTextColorChange(BindableObject bindable, object oldValue, object newValue)
    {
        var radialProgressBarControl = (RadialProgressBarControl)bindable;
        radialProgressBarControl.ProgressLabel.TextColor = (Color)newValue;
    }

    private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var radialProgressBarControl = (RadialProgressBarControl)bindable;
        radialProgressBarControl.ProgressLabel.FontSize = (double)newValue;
    }

    private static void Invalidate(BindableObject bindable, object oldValue, object newValue)
    {
        var radialProgressBarControl = (RadialProgressBarControl)bindable;
        radialProgressBarControl.UpdateProgressNumber();
        radialProgressBarControl.ProgressView.Invalidate();
    }
}