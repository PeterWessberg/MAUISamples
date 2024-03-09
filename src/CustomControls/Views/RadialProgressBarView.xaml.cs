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

    private async void ProgressButton_Clicked(object sender, EventArgs e)
    {

        var acquired = await _mutex.WaitAsync(0); 
        if (!acquired)
            return; 

        try
        {
            RadialProgressBar.Progress += 5;
        }
        finally
        {
            _mutex.Release(); 
        }

        await Task.Delay(100); 
    }

    private void TimerButton_Clicked(object sender, EventArgs e)
    {
        InitializeTimer();
    }

    private async void InitializeTimer()
    {
        // Prevent multiple clicks
        var acquired = await _mutex.WaitAsync(0); 
        if (!acquired)
            return;

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
    private async void AnimateButton_Clicked(object sender, EventArgs e)
    {
        var acquired = await _mutex.WaitAsync(0); 
        if (!acquired)
            return; 

        try
        {
            var firstAnimation = new Animation(v => RadialProgressBar.Progress = v, 0, 90, Easing.Linear);
            var secondAnimation = new Animation(v => RadialProgressBar.Progress = v, 90, 100, Easing.Linear);

            firstAnimation.Commit(this, "FirstAnimation", 16, 2000, Easing.Linear, (v, c) =>
            {
                secondAnimation.Commit(this, "SecondAnimation", 16, 2000, Easing.Linear, (v2, c2) =>
                {
                }, () => false);
            }, () => false);
        }
        finally
        {
            _mutex.Release(); 
        }
    }
}