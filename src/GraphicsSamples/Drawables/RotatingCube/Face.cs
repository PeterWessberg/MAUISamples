using System.Numerics;

namespace GraphicsSamples.Drawables;

public class Face
{
    private int[] vertexIndices;
    private Cube cube;
    
    public Vector3 Normal { get; private set; }

    public Vector3[] Vertices => vertexIndices.Select(index => cube.Vertices[index]).ToArray();

    public int[] VertexIndices { get => vertexIndices; set => vertexIndices = value; }

    public Face(int[] indices, Cube cube)
    {
        vertexIndices = indices;
        this.cube = cube;
        CalculateNormal();
    }

    private void CalculateNormal()
    {
        // Assuming vertices are in correct winding order for a face
        var v1 = cube.Vertices[vertexIndices[1]] - cube.Vertices[vertexIndices[0]];
        var v2 = cube.Vertices[vertexIndices[2]] - cube.Vertices[vertexIndices[0]];
        Normal = Vector3.Normalize(Vector3.Cross(v1, v2));
    }
}




