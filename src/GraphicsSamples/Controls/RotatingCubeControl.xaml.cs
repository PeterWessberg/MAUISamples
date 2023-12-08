using System.Numerics;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GraphicsSamples.Controls;

public partial class RotatingCubeControl : ContentView
{
	public RotatingCubeControl()
	{
		InitializeComponent();
        SpeedSlider.Value = 180;
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

    private void LightXSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        RotatingCubeGraphics.LightSorucePosition = (float)e.NewValue;
    }

    private void AttenuationSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        RotatingCubeGraphics.Attenuation = (float)e.NewValue;
    }

    private void LightIntensitySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        RotatingCubeGraphics.LightIntensity = (float)e.NewValue;
    }
}