using GraphicsSamples.Helpers;
using System.Numerics;

namespace GraphicsSamples.Drawables;

public class RotatingCubeDrawable : IDrawable
{
    private Cube cube = new Cube();
    private Camera camera = new Camera();

    public bool IsRotating { get; set; } = true;
    public Vector3 RotationAngles { get; set; }

    public float RotationSpeed { get; set; }

    public float AngleX { get; set; }
    public float AngleY { get; set; }

    public float Scale { get; set; } = 0.5f;

    public bool IsShadingEnabled { get; set; }

    public bool DoAnimation { get; set; }
    public float Attenuation { get; set; }
    public float DeltaTime { get; set; }

    private Light light = new Light
    {
        Position = new Vector3(5, 5, -5),  
        
        Intensity = new Vector3(0.5f, 0.5f, 0.5f)
    };
    public Light Light
    {
        get => light;
        set => light = value;
        
    }

    private Material material = new Material
    {
        Ambient = new Vector3(0.2f, 0.2f, 0.2f),  
        Diffuse = new Vector3(0.8f, 0.8f, 0.8f),  
        Specular = new Vector3(1.0f, 1.0f, 1.0f), 
        Shininess = 50.0f  
    };

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var baseScale = 1.0f;
        var effectiveScale = baseScale *  Scale;
        canvas.FillColor = Colors.Black;
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;

        canvas.SaveState();

        if (IsRotating && DeltaTime > 0)
        {
            float deltaRotationX = RotationSpeed * DeltaTime; 
            float deltaRotationY = RotationSpeed * DeltaTime; 
            RotationAngles = new Vector3(RotationAngles.X + deltaRotationX, RotationAngles.Y + deltaRotationY, RotationAngles.Z);
            cube.Rotate(RotationSpeed, RotationAngles.X, RotationAngles.Y, RotationAngles.Z);
        }

        cube.ScaleFactor = new Vector3(effectiveScale, effectiveScale, effectiveScale);
        
        DrawCubeWithCamera(canvas, dirtyRect, IsShadingEnabled);
        if (IsShadingEnabled)
        {
            var rotationMatrix = Matrix4x4.CreateRotationY(MathHelper.ToRadians(cube.RotationY));
            var scaleMatrix = Matrix4x4.CreateScale(cube.ScaleFactor);
            var worldMatrix = scaleMatrix * rotationMatrix;

            cube.UpdateShading(camera.Position, light, material, worldMatrix, Attenuation);

        }

        DrawLightSource(canvas, dirtyRect, light.Position);

        canvas.RestoreState();
    }

    private void DrawCubeWithCamera(ICanvas canvas, RectF dirtyRect, bool useShadedColors)
    {
        var viewMatrix = camera.GetViewMatrix();
        var projectionMatrix = camera.GetProjectionMatrix();
        var rotationMatrix = Matrix4x4.CreateRotationY(MathHelper.ToRadians(cube.RotationY));
        var scaleMatrix = Matrix4x4.CreateScale(cube.ScaleFactor);
        var worldMatrix = scaleMatrix * rotationMatrix;

        var faceDepthInfos = cube.Faces.Select(face => new
        {
            Face = face,
            Depth = face.Vertices
                .Select(vertex => Vector3.Transform(vertex, worldMatrix))
                .Select(vertex => Vector3.Transform(vertex, viewMatrix))
                .Average(vertex => vertex.Z) 
        })
        .OrderByDescending(faceInfo => faceInfo.Depth) 
        .ToList();

        foreach (var faceInfo in faceDepthInfos)
        {
            var face = faceInfo.Face;
            var projectedPoints = face.Vertices
                .Select(vertex => Vector3.Transform(vertex, worldMatrix))
                .Select(vertex => Vector3.Transform(vertex, viewMatrix))
                .Select(vertex => Vector3.Transform(vertex, projectionMatrix))
                .Select(vertex => new PointF(
                    (vertex.X + 1) * dirtyRect.Width * 0.5f,
                    (1 - vertex.Y) * dirtyRect.Height * 0.5f))
                .ToArray();

            Color faceColor;
            if (useShadedColors)
            {
                Vector3 faceColorVector = face.VertexIndices
                    .Select(index => cube.ShadedColors[index])
                    .Aggregate((c1, c2) => c1 + c2) / face.VertexIndices.Length;
                faceColor = new Color(faceColorVector.X, faceColorVector.Y, faceColorVector.Z);
            }
            else
            {
                faceColor = Colors.Transparent; 
            }

            DrawFace(canvas, projectedPoints, faceColor);
        }
    }

    private void DrawFace(ICanvas canvas, PointF[] vertices, Color color)
    {
        using (var path = new PathF())
        {
            path.MoveTo(vertices[0]);
            for (int i = 1; i < vertices.Length; i++)
            {
                path.LineTo(vertices[i]);
            }
            path.Close();

            canvas.FillColor = color;
            canvas.StrokeColor = Colors.Black; // Edge color
            canvas.DrawPath(path);
            canvas.FillPath(path);
        }
    }

    private void DrawLightSource(ICanvas canvas, RectF dirtyRect, Vector3 lightPosition)
    {
        var viewMatrix = camera.GetViewMatrix();
        var projectionMatrix = camera.GetProjectionMatrix();

        // Transform the light position from 3D to 2D
        var lightPositionTransformed = Vector3.Transform(lightPosition, viewMatrix);
        lightPositionTransformed = Vector3.Transform(lightPositionTransformed, projectionMatrix);

        if (lightPositionTransformed.Z < 0)
        {
            return; // Light is behind the camera
        }

        var lightPoint = new PointF(
            (lightPositionTransformed.X + 1) * dirtyRect.Width * 0.5f,
            (1 - lightPositionTransformed.Y) * dirtyRect.Height * 0.5f
        );

        canvas.FillColor = Colors.Yellow;
        canvas.FillCircle(lightPoint, 15);
    }
}

public class Light
{
    public Vector3 Position { get; set; }
    public Vector3 Intensity { get; set; } 
}

public class Material
{
    public Vector3 Ambient { get; set; }  // RGB coefficients
    public Vector3 Diffuse { get; set; }  // RGB coefficients
    public Vector3 Specular { get; set; } // RGB coefficients
    public float Shininess { get; set; }  // Shininess coefficient
}
   