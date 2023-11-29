namespace GraphicsSamples.Drawables;

public class WaveBottomDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        Application.Current.Resources.TryGetValue("BlueGreen", out var colorResource);
        Color fillColor = (Color)colorResource;

        PathF path = new();

        float amplitude;
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            amplitude = dirtyRect.Height / 8; // Less wavy for mobile
        }
        else
        {
            amplitude = dirtyRect.Height / 4; // Good amplitude for tablet
        }

        float frequency = (float)(2 * Math.PI / dirtyRect.Width);
        float phase = (float)(-Math.PI / 2 + 0.8);

        // Start the path at the bottom-left corner; 0,0 is in the top left corner.
        path.MoveTo(0, dirtyRect.Height);
        path.LineTo(0, dirtyRect.Height - 50);

        for (float x = 0; x <= dirtyRect.Width; x++)
        {
            float y = (float)(amplitude * Math.Sin(frequency * x + phase)) + dirtyRect.Height / 2;
            path.LineTo(x, y);
        }

        // Close the path at the bottom-right corner
        path.LineTo(dirtyRect.Width, dirtyRect.Height);
        path.Close();

        canvas.FillColor = fillColor;
        canvas.FillPath(path);
        canvas.StrokeSize = 0;
        canvas.DrawPath(path);
    }
}
