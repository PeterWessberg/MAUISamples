using CustomControls.Drawables;

namespace CustomControls.Controls;

public class RadioButtonItem : StackLayout
{
    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(RadioButtonItem), default(double));

    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(RadioButtonItem), default(double), BindingMode.TwoWay);

    public double ImageWidth
    {
        get { return (double)GetValue(ImageWidthProperty); }
        set { SetValue(ImageWidthProperty, value); }
    }

    public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(RadioButtonItem), default(double), BindingMode.TwoWay);

    public double ImageHeight
    {
        get { return (double)GetValue(ImageHeightProperty); }
        set { SetValue(ImageHeightProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioButtonItem), default(string), BindingMode.TwoWay);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty IsCheckedProperty =
         BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButtonItem), default(bool), BindingMode.TwoWay);

    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    public static readonly BindableProperty FontAttributesProperty =
         BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(RadioButtonsControl), FontAttributes.Bold);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButtonItem), Colors.Black);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty DefaultColorProperty =
        BindableProperty.Create(nameof(DefaultColor), typeof(Color), typeof(RadioButtonItem), Colors.Black);

    public Color DefaultColor
    {
        get => (Color)GetValue(DefaultColorProperty);
        set => SetValue(DefaultColorProperty, value);
    }

    //public Color CheckedColor { get; set; }
    public static readonly BindableProperty CheckedColorProperty =
        BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(RadioButtonItem), Colors.Black);

    public Color CheckedColor
    {
        get => (Color)GetValue(CheckedColorProperty);
        set => SetValue(CheckedColorProperty, value);
    }

    public RadioButtonItem()
    {
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        this.SetBinding(TextProperty, new Binding("LabelText"));
        this.SetBinding(DefaultColorProperty, new Binding("DefaultColor"));
        this.SetBinding(CheckedColorProperty, new Binding("CheckedColor"));
        this.SetBinding(IsCheckedProperty, new Binding("IsChecked"));

        var radioButtonGraphicsView = new RadioButtonGraphicsView
        {
            WidthRequest = ImageWidth,
            HeightRequest = ImageHeight,
            IsChecked = IsChecked,
            CheckedColor = CheckedColor,
            DefaultColor = DefaultColor,
        };

        var label = new Label
        {
            FontAttributes = FontAttributes,
            FontSize = FontSize,
            TextColor = TextColor,
            Text = Text
        };

        GestureRecognizers.Add(new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1,
            Command = new Command(() => OnRadioButtonTapped(this))
        });

        Orientation = StackOrientation.Horizontal;
        Spacing = Spacing;

        Children.Add(radioButtonGraphicsView);
        Children.Add(label);
    }

    private async void OnRadioButtonTapped(RadioButtonItem tappedItem)
    {
        if (Parent is RadioButtonsControl parentControl)
        {
            await parentControl.SelectRadioButton(tappedItem);
        }
    }
}