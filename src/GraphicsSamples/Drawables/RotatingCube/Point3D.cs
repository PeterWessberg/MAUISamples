namespace GraphicsSamples.Drawables;

public class Point3D
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Point3D(float X, float Y, float Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    public Point3D Rotate(float[,] matrix)
    {
        float newX = X * matrix[0, 0] + Y * matrix[0, 1] + Z * matrix[0, 2];
        float newY = X * matrix[1, 0] + Y * matrix[1, 1] + Z * matrix[1, 2];
        float newZ = X * matrix[2, 0] + Y * matrix[2, 1] + Z * matrix[2, 2];
        return new Point3D(newX, newY, newZ);
    }

    public PointF ProjectTo2D(Point3D point3D, float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
    {
        float fovRadians = 1.0f / (float)Math.Tan(fieldOfView * 0.5f / 180.0f * Math.PI);
        float x = point3D.X * fovRadians * aspectRatio;
        float y = point3D.Y * fovRadians;
        float z = point3D.Z + nearPlane + farPlane;

        // Perspective division
        if (z != 0)
        {
            x /= z;
            y /= z;
        }

        return new PointF(x, y);
    }
}

