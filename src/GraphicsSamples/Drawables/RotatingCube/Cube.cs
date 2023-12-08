using GraphicsSamples.Helpers;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GraphicsSamples.Drawables;

public class Cube
{
    public Face[] Faces { get; private set; }
    private Point3D[] vertices;
    public Vector3 ScaleFactor { get; set; } = new Vector3(1, 1, 1);
    public Vector3[] ShadedColors { get; private set; }
    public float RotationY { get; set; } = 0;

    public Vector3[] Vertices { get; private set; }

    public Cube()
    {
        Vertices = new Vector3[]
        {
            new Vector3(-1, -1, -1), // Vertex 0
            new Vector3( 1, -1, -1), // Vertex 1
            new Vector3( 1, -1,  1), // Vertex 2
            new Vector3(-1, -1,  1), // Vertex 3
            new Vector3(-1,  1, -1), // Vertex 4
            new Vector3( 1,  1, -1), // Vertex 5
            new Vector3( 1,  1,  1), // Vertex 6
            new Vector3(-1,  1,  1)  // Vertex 7
        };

        Faces = new Face[]
        {
            new Face(new[] { 0, 1, 2, 3 }, this),
            new Face(new[] { 4, 5, 6, 7 }, this),
            new Face(new[] { 0, 1, 5, 4 }, this),
            new Face(new[] { 2, 3, 7, 6 }, this),
            new Face(new[] { 0, 3, 7, 4 }, this),
            new Face(new[] { 1, 2, 6, 5 }, this),
        };

        ShadedColors = new Vector3[Vertices.Length];
        for (int i = 0; i < Vertices.Length; i++)
        {
            ShadedColors[i] = new Vector3(1, 1, 1); // Default to white
        }
    }

    public void Rotate(float rotationSpeed, float angleX, float angleY, float angleZ)
    {
        float radX = MathHelper.ToRadians(angleX * rotationSpeed);
        float radY = MathHelper.ToRadians(angleY * rotationSpeed);
        float radZ = MathHelper.ToRadians(angleZ * rotationSpeed);

        Matrix4x4 rotationMatrixX = Matrix4x4.CreateRotationX(radX);
        Matrix4x4 rotationMatrixY = Matrix4x4.CreateRotationY(radY);
        Matrix4x4 rotationMatrixZ = Matrix4x4.CreateRotationZ(radZ);

        Matrix4x4 combinedRotationMatrix = rotationMatrixX * rotationMatrixY * rotationMatrixZ;

        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] = Vector3.Transform(Vertices[i], combinedRotationMatrix);
        }
    }

    public void UpdateShading(Vector3 viewPosition, Light light, Material material, Matrix4x4 worldMatrix, float attenuation)
    {
        foreach (var face in Faces)
        {
            Vector3 worldNormal = Vector3.Normalize(Vector3.TransformNormal(face.Normal, worldMatrix));
            foreach (var index in face.VertexIndices)
            {
                Vector3 worldVertex = Vector3.Transform(Vertices[index], worldMatrix);
                ShadedColors[index] = CalculatePhongShading(worldVertex, worldNormal, viewPosition, light, material, attenuation);
            }
        }
    }

    public Vector3 CalculatePhongShading(Vector3 vertex, Vector3 normal, Vector3 viewPosition, Light light, Material material, float attenuation)
    {
        normal = Vector3.Normalize(normal);
        Vector3 lightDir = Vector3.Normalize(light.Position - vertex);
        Vector3 viewDir = Vector3.Normalize(viewPosition - vertex);

        Vector3 ambient = material.Ambient * light.Intensity;

        float distance = Vector3.Distance(vertex, light.Position);
        //float attenuation = 1.0f / (1.0f + 0.09f * distance + 0.032f * distance * distance);
        float diff = Math.Max(Vector3.Dot(normal, lightDir), 0.0f);
        Vector3 diffuse = material.Diffuse * diff * light.Intensity * attenuation;

        Vector3 reflectDir = Vector3.Reflect(-lightDir, normal);
        float spec = (float)Math.Pow(Math.Max(Vector3.Dot(viewDir, reflectDir), 0.0f), material.Shininess);
        Vector3 specular = material.Specular * spec * light.Intensity * attenuation;

        Vector3 finalColor = ambient + diffuse + specular;
        finalColor = Vector3.Clamp(finalColor, Vector3.Zero, Vector3.One);

        return finalColor;
    }

    private Vector3 CalculateNormalForVertex(Vector3 vertex)
    {
        if (vertex.X == -1) return new Vector3(-1, 0, 0); // Left face
        if (vertex.X == 1) return new Vector3(1, 0, 0);   // Right face
        if (vertex.Y == -1) return new Vector3(0, -1, 0); // Bottom face
        if (vertex.Y == 1) return new Vector3(0, 1, 0);   // Top face
        if (vertex.Z == -1) return new Vector3(0, 0, -1); // Front face
        if (vertex.Z == 1) return new Vector3(0, 0, 1);   // Back face

        return Vector3.UnitY; // Default normal
    }


    public void ApplyTransformation(Matrix4x4 transformation)
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            // Apply the transformation matrix, including rotation around Y-axis
            Vertices[i] = Vector3.Transform(Vertices[i], transformation);
        }
    }

    private float[,] MultiplyMatrices(float[,] matrixA, float[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int rowsB = matrixB.GetLength(0);
        int colsB = matrixB.GetLength(1);

        if (colsA != rowsB)
        {
            throw new InvalidOperationException();
        }

        float[,] resultMatrix = new float[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                resultMatrix[i, j] = 0;
                for (int k = 0; k < colsA; k++)
                {
                    resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        }

        return resultMatrix;
    }
}


