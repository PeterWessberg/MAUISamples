using System.Numerics;

namespace GraphicsSamples.Drawables;

public class RotatingCubeGraphics : GraphicsView
{
    private const int BaseUpdateInterval = 16; // Base for 60 FPS
    private double dynamicUpdateInterval;
    private DateTime lastUpdate;
    private CancellationTokenSource cts;
    private RotatingCubeDrawable drawable;
    private Vector3 lastRotationAngles;
    public EventHandler<PanUpdatedEventArgs> Panning;
    public EventHandler<PanUpdatedEventArgs> PanningCompleted;

    private double rotationSpeed = 0;
    public double RotationSpeed
    {
        get => rotationSpeed;
        set
        {
            rotationSpeed = value;
            drawable.RotationSpeed = (float)value;
        }
    }

    private bool isShadingEnabled = false;
    public bool IsShadingEnabled
    {
        get => isShadingEnabled;
        set
        {
            isShadingEnabled = value;
            drawable.IsShadingEnabled = value;
            if (!drawable.IsRotating) Invalidate();
        }
    }
    private bool doAnimation = false;
    public bool DoAnimation
    {
        get => doAnimation;
        set
        {
            if (value)
            {
                drawable.IsRotating = true;
                drawable.RotationAngles = lastRotationAngles; // Resume from last known angles
                StartAnimation();
            }
            else
            {
                cts.Cancel();
                drawable.IsRotating = false;
                lastRotationAngles = drawable.RotationAngles; // Save current rotation angles
            }
            doAnimation = value;
            drawable.DoAnimation = value;
        }
    }

    private float lightSourccePosition;
    public float LightSorucePosition
    {
        get => lightSourccePosition;
        set
        {
            lightSourccePosition = value;
            drawable.Light.Position = new Vector3(value, drawable.Light.Position.Y, drawable.Light.Position.Z);

            if (!drawable.IsRotating) Invalidate();
        }
    }
    private float lightIntensity;
    public float LightIntensity
    {
        get => lightIntensity;
        set
        {
            lightIntensity = value;
            drawable.Light.Intensity = new Vector3(value, value, value);
            if (!drawable.IsRotating) Invalidate();
        }
    }
    private float attenuation;
    public float Attenuation
    {
        get => attenuation;
        set
        {
            attenuation = value;
            drawable.Attenuation = value;
            if (!drawable.IsRotating) Invalidate();
        }
    }

    public RotatingCubeGraphics()
    {
        drawable = new RotatingCubeDrawable();
        drawable.AngleX = 20;
        drawable.AngleY = 20;
        Drawable = drawable;

        PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        this.GestureRecognizers.Add(pinchGesture);
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
        dynamicUpdateInterval = BaseUpdateInterval;
        StartAnimation();
    }

    private void StartAnimation()
    {
        lastUpdate = DateTime.UtcNow;
        drawable.RotationSpeed = (float)rotationSpeed;
        cts = new CancellationTokenSource();
        _ = UpdateCanvas();
    }

    private async Task UpdateCanvas()
    {
        while (!cts.Token.IsCancellationRequested)
        {
            var currentUpdate = DateTime.UtcNow;
            var timeSinceLastUpdate = (currentUpdate - lastUpdate).TotalMilliseconds;

            if (timeSinceLastUpdate >= dynamicUpdateInterval)
            {
                lastUpdate = currentUpdate;
                drawable.DeltaTime = (float)timeSinceLastUpdate / 1000f;
                Invalidate();
                AdjustFrameRate(timeSinceLastUpdate);


                var processingTime = (DateTime.UtcNow - currentUpdate).TotalMilliseconds;
                var delay = Math.Max(dynamicUpdateInterval - processingTime, 0);
                await Task.Delay((int)delay);
            }
            else
            {
                await Task.Delay((int)(dynamicUpdateInterval - timeSinceLastUpdate));
            }
        }
    }

    private void AdjustFrameRate(double actualFrameDuration)
    {
        if (actualFrameDuration > dynamicUpdateInterval * 1.1) // frame takes longer than expected
        {
            dynamicUpdateInterval = Math.Min(dynamicUpdateInterval + 1, 33); // Lower frame rate, but not below 30 FPS
        }
        else if (actualFrameDuration < dynamicUpdateInterval * 0.9) // frame is faster than expected
        {
            dynamicUpdateInterval = Math.Max(dynamicUpdateInterval - 1, BaseUpdateInterval); // Increase frame rate, up to 60 FPS
        }
    }
    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        switch (e.Status)
        {
            case GestureStatus.Started:
                drawable.IsRotating = false;
                break;

            case GestureStatus.Running:
                drawable.Scale *= (float)e.Scale;
                Invalidate();
                break;

            case GestureStatus.Completed:
                drawable.IsRotating = true;
                break;
        }
    }
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                drawable.IsRotating = true;
                break;
            case GestureStatus.Running:
                float sensitivityX = 0.1f;
                float sensitivityY = 0.1f;

                drawable.AngleY += (float)e.TotalY * sensitivityY;
                drawable.AngleX += (float)e.TotalX * sensitivityX;

                Panning?.Invoke(sender, e);
                Invalidate();
                break;
            case GestureStatus.Completed:
                drawable.IsRotating = false;
                PanningCompleted?.Invoke(sender, e);
                break;
            case GestureStatus.Canceled:
                break;
            default:
                break;
        }
    }
}
