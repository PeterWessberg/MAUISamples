namespace GraphicsSamples.Drawables;

public static class ColorExtensions
{
    public static Color MultiplyBrightness(this Color color, float brightnessFactor)
    {
        float red = Math.Clamp(color.Red * brightnessFactor, 0, 1);
        float green = Math.Clamp(color.Green * brightnessFactor, 0, 1);
        float blue = Math.Clamp(color.Blue * brightnessFactor, 0, 1);

        return new Color(red, green, blue, color.Alpha);
    }
}
