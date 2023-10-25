using CustomControls.Drawables;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CustomControls.Controls;

public class RadioButtonsControl : StackLayout
{
    public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RadioButtonsControl), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButtonsControl), null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty =
    BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonsControl), default(object),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: SelectedItemPropertyChanged);

    private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty ButtonsItemsProperty =
        BindableProperty.Create(nameof(ButtonsItems), typeof(List<ControlButton>), typeof(RadioButtonsControl), null, BindingMode.OneWay, propertyChanged: (bindable, newValue, oldValue) =>
        {
            var control = (RadioButtonsControl)bindable;
            //BindableLayout.SetItemsSource(control, control.ButtonsLabels);
        });

    public List<ControlButton> ButtonsItems
    {
        get => (List<ControlButton>)GetValue(ButtonsItemsProperty);
        set => SetValue(ButtonsItemsProperty, value);
    }

    public static readonly BindableProperty AnimateProperty =
        BindableProperty.Create(nameof(Animate), typeof(bool), typeof(RadioButtonsControl), false);

    public bool Animate
    {
        get => (bool)GetValue(AnimateProperty);
        set => SetValue(AnimateProperty, value);
    }

    public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(RadioButtonsControl), 20.0);

    public double ImageWidth
    {
        get => (double)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(RadioButtonsControl), 20.0);

    public double ImageHeight
    {
        get => (double)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(RadioButtonsControl), FontAttributes.Bold);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(RadioButtonsControl), 16.0);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButtonsControl), Colors.Black);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    public ICommand SelectRadioButtonCommand { get; }

    private DataTemplate dataTemplate;

    public event Action AnimationCompleted;

    public event Action AnimationStarted;

    public RadioButtonsControl()
    {
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        // Setting the DataTemplate, DataSource and the properties that is common throughout the control
        dataTemplate = new DataTemplate(typeof(RadioButtonItem));
        dataTemplate.SetValue(RadioButtonItem.FontSizeProperty, FontSize);
        dataTemplate.SetValue(RadioButtonItem.ImageWidthProperty, ImageWidth);
        dataTemplate.SetValue(RadioButtonItem.ImageHeightProperty, ImageWidth);
        dataTemplate.SetValue(RadioButtonItem.TextColorProperty, TextColor);
        dataTemplate.SetValue(RadioButtonItem.FontAttributesProperty, FontAttributes);
        BindableLayout.SetItemTemplate(this, dataTemplate);
        BindableLayout.SetItemsSource(this, ButtonsItems);
    }

    public async Task SelectRadioButton(RadioButtonItem radioButton)
    {
        int index = 0;
        int startIndex = 0;
        int endIndex = 0;
        // Iterate through the list of buttons and set index and the new select
        foreach (RadioButtonItem item in Children)
        {
            if (item.Children[0] is RadioButtonGraphicsView view)
            {
                if (view.IsChecked) startIndex = index;

                if (item.Id == radioButton.Id)
                {
                    view.IsChecked = true;
                    CheckedChanged?.Invoke(item, new CheckedChangedEventArgs(true));
                    SelectedItem = item;
                    endIndex = index;
                }
                else
                {
                    view.IsChecked = false;
                }
            }
            index++;
        }

        // We now have the start index and end index in the list. Let's animate.
        AnimationStarted?.Invoke();
        bool movingDown = startIndex < endIndex;
        for (int i = startIndex; movingDown ? i <= endIndex : i >= endIndex; i += movingDown ? 1 : -1)
        {
            if (startIndex == endIndex) break;
            if (Children[i] is RadioButtonItem item && item.Children[0] is RadioButtonGraphicsView view)
            {
                MovementDirection direction = (movingDown, i) switch
                {
                    (true, var idx) when idx == startIndex => MovementDirection.MiddleToDown,
                    (true, var idx) when idx == endIndex => MovementDirection.TopToMiddle,
                    (true, _) => MovementDirection.PassingDown,
                    (false, var idx) when idx == startIndex => MovementDirection.MiddleToUp,
                    (false, var idx) when idx == endIndex => MovementDirection.BottomToMiddle,
                    (false, _) => MovementDirection.PassingUp
                };
                await view.StartMovementAnimation(direction);
            }
        }
        AnimationCompleted?.Invoke();

        if (Command != null)
        {
            if (Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        await Task.CompletedTask;
    }
}

public partial class ControlButton : INotifyPropertyChanged
{
    public string LabelText { get; set; } = string.Empty;

    public object Value { get; set; }

    public int Index { get; set; }

    private bool isChecked = false;

    public Color DefaultColor { get; set; } = Colors.Black;

    public Color CheckedColor { get; set; } = Colors.Black;

    public bool IsChecked
    {
        get => isChecked;
        set
        {
            isChecked = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}