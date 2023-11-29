namespace GraphicsSamples.Interface;

public interface IPinchGestureService
{
    void AddPinchGesture(GraphicsView view, Action<double> onPinch);
}
