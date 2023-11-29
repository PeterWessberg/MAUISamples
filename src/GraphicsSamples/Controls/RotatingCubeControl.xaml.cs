using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GraphicsSamples.Controls;

public partial class RotatingCubeControl : ContentView
{
	public RotatingCubeControl()
	{
		InitializeComponent();
        SpeedSlider.Value = 90;
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double mappedSpeed = Math.Pow(e.NewValue / 360.0, 2);
        RotatingCubeGraphics.RotationSpeed = mappedSpeed;
    }

    private void ShadingToggle_Toggled(object sender, ToggledEventArgs e)
    {
        RotatingCubeGraphics.IsShadingEnabled = e.Value;
    }

    private void AnimationToggle_Toggled(object sender, ToggledEventArgs e)
    {
        RotatingCubeGraphics.DoAnimation = e.Value;
    }
}