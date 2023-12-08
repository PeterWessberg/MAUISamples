using GraphicsSamples.Helpers;
using System.Numerics;

namespace GraphicsSamples.Drawables;

public class Camera
{
    public Vector3 Position { get; set; } = new Vector3(0, 0, 5);
    public Vector3 Target { get; set; } = Vector3.Zero;
    public Vector3 UpDirection { get; set; } = Vector3.UnitY;
    public float FieldOfView { get; set; } = 90.0f;
    public float AspectRatio { get; set; } = 1.0f;
    public float NearPlane { get; set; } = 0.1f;
    public float FarPlane { get; set; } = 10.0f;

    public Matrix4x4 GetViewMatrix()
    {
        return Matrix4x4.CreateLookAt(Position, Target, UpDirection);
    }

    public Matrix4x4 GetProjectionMatrix()
    {
        return Matrix4x4.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(FieldOfView), AspectRatio, NearPlane, FarPlane);
    }
}

