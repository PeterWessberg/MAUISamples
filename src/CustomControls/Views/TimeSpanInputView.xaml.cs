namespace CustomControls.Views;

public partial class TimeSpanInputView : ContentPage
{
    public TimeSpanInputView()
    {
        InitializeComponent();
        TimeSpanControl.DurationChanged += OnDurationChanged;
        TimeLabel.Text = DateTime.Now.ToShortTimeString();
    }

    private void OnDurationChanged(TimeSpan span)
    {
        var timeToChange = DateTime.Parse(TimeLabel.Text);
        ReturnValueLabel.Text = "New Time is: " + timeToChange.Add(span).ToShortTimeString();
    }
}