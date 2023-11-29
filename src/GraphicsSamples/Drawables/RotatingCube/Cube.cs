namespace GraphicsSamples.Drawables;

public class Cube
{
    public Face[] Faces { get; private set; }
    private Point3D[] vertices;

    public Point3D[] Vertices
    {
        get => vertices; 
    }

    public Cube()
    {
        vertices = new Point3D[]
        {
            new Point3D(-1, -1, -1),
            new Point3D(1, -1, -1),
            new Point3D(1, 1, -1),
            new Point3D(-1, 1, -1),
            new Point3D(-1, -1, 1),
            new Point3D(1, -1, 1),
            new Point3D(1, 1, 1),
            new Point3D(-1, 1, 1)
        };

        Faces = new Face[]
        {
            new Face(new[] { 0, 1, 2, 3 }, vertices),
            new Face(new[] { 4, 5, 6, 7 }, vertices),
            new Face(new[] { 0, 1, 5, 4 }, vertices),
            new Face(new[] { 2, 3, 7, 6 }, vertices),
            new Face(new[] { 0, 3, 7, 4 }, vertices),
            new Face(new[] { 1, 2, 6, 5 }, vertices),
        };
    }

    public void Rotate(float rotationSpeed, float angleX, float angleY, float angleZ)
    {
        float radX = (angleX * rotationSpeed) * (float)Math.PI / 180;
        float radY = (angleY * rotationSpeed) * (float)Math.PI / 180;
        float radZ = (angleZ * rotationSpeed) * (float)Math.PI / 180;

        // Precompute the rotation matrices
        float[,] rotX = new float[,]
        {
            { 1, 0, 0 },
            { 0, (float)Math.Cos(radX), -(float)Math.Sin(radX) },
            { 0, (float)Math.Sin(radX), (float)Math.Cos(radX) }
        };
        float[,] rotY = new float[,]
        {
            { (float)Math.Cos(radY), 0, (float)Math.Sin(radY) },
            { 0, 1, 0 },
            { -(float)Math.Sin(radY), 0, (float)Math.Cos(radY) }
        };
        float[,] rotZ = new float[,]
        {
            { (float)Math.Cos(radZ), -(float)Math.Sin(radZ), 0 },
            { (float)Math.Sin(radZ), (float)Math.Cos(radZ), 0 },
            { 0, 0, 1 }
        };

        var combinedRotationMatrix = MultiplyMatrices(MultiplyMatrices(rotX, rotY), rotZ);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].Rotate(combinedRotationMatrix);
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


