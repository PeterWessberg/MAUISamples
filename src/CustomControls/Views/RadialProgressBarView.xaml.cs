using System.Timers;

namespace CustomControls.Views;

public partial class RadialProgressBarView : ContentPage
{
    private System.Timers.Timer timer;
    private SemaphoreSlim _mutex = new(1, 1);

    public RadialProgressBarView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void ProgressButton_Clicked(object sender, EventArgs e)
    {
        RadialProgressBar.Progress += 5;
        Task.Delay(100);
    }

    private void TimerButton_Clicked(object sender, EventArgs e)
    {
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        // Prevent multiple clicks
        if (_mutex.CurrentCount == 0)
        {
            return;
        }
        _mutex.Wait();

        RadialProgressBar.Progress = 0;
        timer = new System.Timers.Timer(500);
        timer.Elapsed += new ElapsedEventHandler(RadialProgressBar_Progress);
        timer.Start();
    }

    private void RadialProgressBar_Progress(object sender, ElapsedEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            RadialProgressBar.Progress += 5;

            if (RadialProgressBar.Progress == 100)
            {
                timer.Stop();
                _mutex.Release();
            }
        });
    }

    /// <summary>
    /// Run animations, first 0-90% for 2 seconds and then the last 10% units for 2 seconds. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AnimateButton_Clicked(object sender, EventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            var firstAnimation = new Animation(v => RadialProgressBar.Progress = v, 0, 90);
            var secondAnimation = new Animation(v => RadialProgressBar.Progress = v, 90, 100);
            firstAnimation.Commit(RadialProgressBar, "FirstAnimation", 16, 2000, Easing.Linear, (v, c) =>
            {
                secondAnimation.Commit(RadialProgressBar, "SecondAnimation", 16, 2000, Easing.Linear);
            });
        });
    }
}