using System.Numerics;

namespace GraphicsSamples.Drawables;

public class Face
{
    private int[] vertexIndices;
    private Point3D[] cubeVertices;

    public Point3D[] Vertices => vertexIndices.Select(index => cubeVertices[index]).ToArray();

    public Color FaceColor { get; set; }

    public Face(int[] indices, Point3D[] globalVertices)
    {
        vertexIndices = indices;
        cubeVertices = globalVertices;
    }

    public Vector3 CalculateNormal()
    {
        Vector3 normal = new Vector3(0, 0, 0);

        for (int i = 0; i < Vertices.Length; i++)
        {
            Point3D current = Vertices[i];
            Point3D next = Vertices[(i + 1) % Vertices.Length];

            normal.X += (current.Y - next.Y) * (current.Z + next.Z);
            normal.Y += (current.Z - next.Z) * (current.X + next.X);
            normal.Z += (current.X - next.X) * (current.Y + next.Y);
        }

        return Vector3.Normalize(normal);
    }
}

