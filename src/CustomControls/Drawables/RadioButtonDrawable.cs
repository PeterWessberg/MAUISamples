using CustomControls.Controls;
using CustomControls.Helpers;
using Microsoft.Maui.Animations;

namespace CustomControls.Drawables;

public class RadioButtonGraphicsView : GraphicsView
{
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButtonGraphicsView), false, propertyChanged: OnIsCheckedChanged);

    public static readonly BindableProperty DefaultColorProperty =
        BindableProperty.Create(nameof(DefaultColor), typeof(Color), typeof(RadioButtonGraphicsView), Colors.Black);

    public static readonly BindableProperty CheckedColorProperty =
        BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(RadioButtonGraphicsView), Colors.Black);

    public static readonly BindableProperty IndexProperty =
        BindableProperty.Create(nameof(Index), typeof(int), typeof(RadioButtonGraphicsView));

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public Color DefaultColor
    {
        get => (Color)GetValue(DefaultColorProperty);
        set => SetValue(DefaultColorProperty, value);
    }

    public Color CheckedColor
    {
        get => (Color)GetValue(CheckedColorProperty);
        set => SetValue(CheckedColorProperty, value);
    }

    private readonly RadioButtonDrawable _drawable;

    private IAnimationManager _animationManager;

    public RadioButtonGraphicsView()
    {
        _drawable = new RadioButtonDrawable(this);
        Initialize();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        var parentControl = FindParent<RadioButtonsControl>(this);
        if (parentControl != null)
        {
            parentControl.AnimationStarted += () => _drawable.Animating = true;
            parentControl.AnimationCompleted += () => _drawable.Animating = false;
        }
        var parentItem = FindParent<RadioButtonItem>(this);
        this.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => await parentControl.SelectRadioButton(parentItem))
        });
    }

    protected void Initialize()
    {
        _animationManager = AppServiceProvider.GetService<IAnimationManager>();
        Drawable = _drawable;
    }

    private T FindParent<T>(Element element) where T : Element
    {
        var parent = element.Parent;
        while (parent != null)
        {
            if (parent is T target)
            {
                return target;
            }
            parent = parent.Parent;
        }
        return null;
    }

    private static async void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((bool)newValue)
        {
            await ((RadioButtonGraphicsView)bindable).AnimateRippleEffect();
        }
    }

    internal async Task AnimateRippleEffect()
    {
        float start = 0;
        float end = 1;

        _animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            _drawable.AnimationPercent = start.Lerp(end, progress);
            Invalidate();
        }, duration: 0.4, easing: Easing.SinInOut,
        finished: () =>
        {
            _drawable.AnimationPercent = 0;
        }));

        await Task.CompletedTask;
    }

    internal async Task StartMovementAnimation(MovementDirection direction)
    {
        await Task.Delay(200);
        float start = 0;
        float end = 1;

        switch (direction)
        {
            case MovementDirection.PassingUp:
                start = 1;
                end = -0.1f;
                break;

            case MovementDirection.PassingDown:
                start = 0;
                end = 1.1f;
                break;

            case MovementDirection.MiddleToUp:
                start = 0.5f;
                end = -0.1f;
                break;

            case MovementDirection.MiddleToDown:
                start = 0.5f;
                end = 1.1f;
                break;

            case MovementDirection.TopToMiddle:
                start = 0;
                end = 0.5f;
                break;

            case MovementDirection.BottomToMiddle:
                start = 1;
                end = 0.5f;
                break;

            default:
                return;
        }

        _animationManager.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            _drawable.AnimationOffsetMoveing = start.Lerp(end, progress);
            _drawable.Animating = true;
            Invalidate();
        }, duration: 0.3, easing: Easing.SinInOut,
        finished: () =>
        {
        }));

        await Task.CompletedTask;
    }
}

public class RadioButtonDrawable : IDrawable
{
    private readonly RadioButtonGraphicsView radioButtonGraphicsView;

    private bool _animate = false;

    public bool Animate
    {
        get => _animate;
        set => _animate = value;
    }

    private bool _animating = false;

    public bool Animating
    {
        get => _animating;
        set => _animating = value;
    }

    private double _animationPercent;

    public double AnimationPercent
    {
        get => _animationPercent;
        set => _animationPercent = value;
    }

    private float _animationOffsetMoveing = -0.5f;

    public float AnimationOffsetMoveing
    {
        get => _animationOffsetMoveing;
        set => _animationOffsetMoveing = value;
    }

    public RadioButtonDrawable(RadioButtonGraphicsView radioButtonGraphicsView) : base()
    {
        this.radioButtonGraphicsView = radioButtonGraphicsView;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        var center = new PointF((float)dirtyRect.Width / 2, (float)dirtyRect.Height / 2);
        var radius = Math.Min((float)dirtyRect.Width, (float)dirtyRect.Height) / 2 - 4;

        // Clipping so that the movement not showing up outside the outer circle.
        var path = new PathF();
        path.AppendCircle(center, radius);
        canvas.ClipPath(path);

        DrawMovement(canvas, dirtyRect);

        canvas.StrokeColor = radioButtonGraphicsView.IsChecked ? radioButtonGraphicsView.CheckedColor : radioButtonGraphicsView.DefaultColor;
        canvas.StrokeSize = 2;
        canvas.DrawCircle(center.X, center.Y, radius);

        if (radioButtonGraphicsView.IsChecked && !_animating)
        {
            canvas.FillColor = radioButtonGraphicsView.CheckedColor;
            canvas.FillCircle(center.X, center.Y, radius - 4);
        }

        // RestoreState removes clipping so that we can get the ripple effect outside the outer circle
        canvas.RestoreState();

        if (radioButtonGraphicsView.IsChecked)
        {
            DrawRippleEffect(canvas, dirtyRect, center);
        }
    }

    internal void DrawRippleEffect(ICanvas canvas, RectF dirtyRect, PointF center)
    {
        canvas.SaveState();

        canvas.FillColor = radioButtonGraphicsView.CheckedColor.WithAlpha(0.75f * (1 - (float)_animationPercent));

        canvas.Alpha = 0.55f;

        float minimumRippleEffectSize = 0.0f;

        var rippleEffectSize = minimumRippleEffectSize.Lerp(dirtyRect.Width * 0.65f, _animationPercent);

        canvas.FillCircle((float)center.X, (float)center.Y, rippleEffectSize);

        canvas.RestoreState();
    }

    internal void DrawMovement(ICanvas canvas, RectF dirtyRect)
    {
        if (!_animating) return;

        var radius = Math.Min((float)dirtyRect.Width, (float)dirtyRect.Height) / 2 - 4;

        var center = new PointF((float)dirtyRect.Width / 2,
                                (float)dirtyRect.Height * (float)_animationOffsetMoveing);

        canvas.FillColor = radioButtonGraphicsView.CheckedColor;
        canvas.FillCircle(center.X, center.Y, radius - 4);
    }
}

public enum MovementDirection
{
    PassingUp,
    PassingDown,
    MiddleToUp,
    MiddleToDown,
    TopToMiddle,
    BottomToMiddle
}