using System.Numerics;

namespace GraphicsSamples.Drawables;

public class RotatingCubeDrawable : IDrawable
{
    private Cube cube = new Cube();

    private float angle = 25;
    private float fieldOfView = 90f; // Degrees
    private float aspectRatio = 1; // dirtyRect.Width / dirtyRect.Height;
    private float nearPlane = 1f;
    private float farPlane = 10f;

    public float Angle
    {
        get => angle;
        set
        {
            angle = value;
            cube.Rotate(RotationSpeed, angle, angle, angle);
        }
    }

    public bool IsRotating { get; set; } = true;

    public float RotationSpeed { get; set; }

    public float AngleX { get; set; }
    public float AngleY { get; set; }

    public float Scale { get; set; } = 2.0f;

    public bool IsShadingEnabled { get; set; }

    public bool DoAnimation { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var baseScale = 500;
        var effectiveScale = baseScale * Scale;
        canvas.FillColor = Colors.Black;
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;

        canvas.SaveState();

        if (IsRotating )
        {
            cube.Rotate(RotationSpeed, AngleX, AngleY, 0);
        }

        //DrawLines(canvas, dirtyRect, effectiveScale); // Just the wire frame
        Shading(canvas, dirtyRect, effectiveScale);
        PrintDot(canvas, dirtyRect, effectiveScale);

        canvas.RestoreState();

    }

    private void DrawLines(ICanvas canvas, RectF dirtyRect, float effectiveScale)
    {
        var projectedPoints = cube.Vertices
            .Select(v => v.ProjectTo2D(v, fieldOfView, aspectRatio, nearPlane, farPlane))
            .Select(p => new PointF(
                p.X * effectiveScale + dirtyRect.Width / 2,
                p.Y * effectiveScale + dirtyRect.Height / 2))
            .ToArray();

        var edges = new (int, int)[]
        {
            (0, 1), (1, 2), (2, 3), (3, 0), // Bottom Face
            (4, 5), (5, 6), (6, 7), (7, 4), // Top Face
            (0, 4), (1, 5), (2, 6), (3, 7)  // Vertical Edges
        };

        foreach (var (start, end) in edges)
        {
            canvas.DrawLine(projectedPoints[start], projectedPoints[end]);
        }
    }

    private void Shading(ICanvas canvas, RectF dirtyRect, float effectiveScale)
    {
        foreach (var face in cube.Faces)
        {
            // Projecting each vertex of the face from 3D to 2D, then applying scaling and centering
            var points = face.Vertices
                .Select(v => v.ProjectTo2D(v, fieldOfView, aspectRatio, nearPlane, farPlane))
                .Select(p => new PointF(
                    p.X * effectiveScale + dirtyRect.Width / 2,
                    p.Y * effectiveScale + dirtyRect.Height / 2))
                .ToArray();

            // Apply shading or default color
            var faceColor = IsShadingEnabled ? CalculateFaceColor(face) : Colors.Transparent;
            DrawPolygon(canvas, points, faceColor);
        }
    }


    private Color CalculateFaceColor(Face face)
    {
        var normal = face.CalculateNormal();
        var lightDirection = new Vector3(1, 1, 1); 

        normal = Vector3.Normalize(normal);
        lightDirection = Vector3.Normalize(lightDirection);

        var dotProduct = Vector3.Dot(normal, lightDirection);
        var brightness = Math.Max(dotProduct, 0); 

        return Colors.White.MultiplyBrightness(brightness);
    }


    private void DrawPolygon(ICanvas canvas, PointF[] points, Color color)
    {
        using (var path = new PathF())
        {
            path.MoveTo(points[0]);
            for (int i = 1; i < points.Length; i++)
            {
                path.LineTo(points[i]);
            }
            path.Close();

            canvas.FillColor = color;
            canvas.FillPath(path);

            canvas.StrokeColor = Colors.Black;
            canvas.DrawPath(path);
        }
    }

    private void PrintDot(ICanvas canvas, RectF dirtyRect, float scale)
    {
        var faceCenters = new PointF[]
        {
            GetFaceCenter(new[] { cube.Vertices[0], cube.Vertices[1], cube.Vertices[2], cube.Vertices[3] }), // Bottom face
            GetFaceCenter(new[] { cube.Vertices[4], cube.Vertices[5], cube.Vertices[6], cube.Vertices[7] }), // Top face
            GetFaceCenter(new[] { cube.Vertices[0], cube.Vertices[1], cube.Vertices[5], cube.Vertices[4] }), // Front face
            GetFaceCenter(new[] { cube.Vertices[2], cube.Vertices[3], cube.Vertices[7], cube.Vertices[6] }), // Back face
            GetFaceCenter(new[] { cube.Vertices[0], cube.Vertices[3], cube.Vertices[7], cube.Vertices[4] }), // Left face
            GetFaceCenter(new[] { cube.Vertices[1], cube.Vertices[2], cube.Vertices[6], cube.Vertices[5] })  // Right face
        };

        var colors = new Color[]
        {
            Colors.Green,
            Colors.Purple,
            Colors.Blue,
            Colors.Yellow,
            Colors.Magenta,
            Colors.DeepPink
        };

        var i = 0;
        foreach (var center in faceCenters)
        {
            canvas.FillColor = colors[i % 6];
            var scaledCenter = new PointF(center.X * scale + dirtyRect.Width / 2, center.Y * scale + dirtyRect.Height / 2);
            canvas.FillCircle(scaledCenter, 5);
            i++;
        }
    }

    private PointF GetFaceCenter(Point3D[] vertices)
    {
        float x = 0, y = 0, z = 0;
        foreach (var v in vertices)
        {
            x += v.X;
            y += v.Y;
            z += v.Z;
        }
        x /= vertices.Length;
        y /= vertices.Length;
        z /= vertices.Length;

        return vertices[0].ProjectTo2D(new Point3D(x, y, z), fieldOfView, aspectRatio, nearPlane, farPlane);
    }
}
