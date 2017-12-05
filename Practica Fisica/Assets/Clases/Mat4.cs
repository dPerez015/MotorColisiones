using UnityEngine;
using System.Collections;

public class Mat3
{
    public float[,] matrix = new float[3, 3];

    public Mat3(float[,] values)
    {
        /*if(values.Length != 9)
        {
            Debug.Log("Invalid input values for the Mat3 initialisation");
            return;
        }*/
        matrix = values;
    }
    public static Mat3 operator *(Mat3 matrixA, Mat3 matrixB)
    {
        float[,] a = matrixA.matrix;
        float[,] b = matrixB.matrix;
        Mat3 newMatrix = new Mat3(new float[,] {    {a[0, 0] * b[0, 0] + a[0, 1] * b[1, 0] + a[0, 2] * b[2, 0],     a[0, 0] * b[0, 1] + a[0, 1] * b[1, 1] + a[0, 2] * b[2, 1],      a[0, 0] * b[0, 2] + a[0, 1] * b[1, 2] + a[0, 2] * b[2, 2]},
                                                    {a[1, 0] * b[0, 0] + a[1, 1] * b[1, 0] + a[1, 2] * b[2, 0],     a[1, 0] * b[0, 1] + a[1, 1] * b[1, 1] + a[1, 2] * b[2, 1],      a[1, 0] * b[0, 2] + a[1, 1] * b[1, 2] + a[1, 2] * b[2, 2]},
                                                    {a[2, 0] * b[0, 0] + a[2, 1] * b[1, 0] + a[2, 2] * b[2, 0],     a[2, 0] * b[0, 1] + a[2, 1] * b[1, 1] + a[2, 2] * b[2, 1],      a[2, 0] * b[0, 2] + a[2, 1] * b[1, 2] + a[2, 2] * b[2, 2]} });
        return newMatrix;
    }
    public static Vec3 operator *(Mat3 matrix, Vec3 vector)
    {
        float[,] a = matrix.matrix;
        Vec3 newVector = new Vec3(a[0, 0] * vector.x + a[0, 1] * vector.x + a[0, 2] * vector.z ,
                                  a[1, 0] * vector.x + a[1, 1] * vector.y + a[1, 2] * vector.z ,
                                  a[2, 0] * vector.x + a[2, 1] * vector.y + a[2, 2] * vector.z);
        return newVector;
    }
    public static Vec3 operator *(Vec3 vector, Mat3 matrix)
    {
        return matrix * vector;
    }

    public static Mat3 operator *(Mat3 matrix, float value)
    {
        Mat3 newMatrix = matrix;
        for (int i = 0; i < matrix.matrix.Length; i++)
        {
            newMatrix.matrix[i / 3, i % 3] *= value;
        }
        return newMatrix;
    }

    public static Mat3 operator *(float value, Mat3 matrix)
    {
        return matrix * value;
    }
}
