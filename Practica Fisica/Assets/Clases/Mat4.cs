﻿using UnityEngine;
using System.Collections;

public class Mat3
{
    public float[,] matrix = new float[3, 3];
    public Mat3()
    {
        matrix = new float[,]
        {
            {1f, 0f, 0f },
            {0f, 1f, 0f },
            {0f, 0f, 1f }
        };
    }
    public Mat3(float[,] values)
    {
        /*if(values.Length != 9)
        {
            Debug.Log("Invalid input values for the Mat3 initialisation");
            return;
        }*/
        matrix = values;
    }
    public Mat3(Vec3 v1, Vec3 v2, Vec3 v3)
    {
        matrix = new float[,]
        {
            {v1.x,v2.x,v3.x},
            {v1.y,v2.y,v3.y},
            {v1.z,v2.z,v3.z}
        };
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
        Vec3 newVector = new Vec3(a[0, 0] * vector.x + a[0, 1] * vector.y + a[0, 2] * vector.z,
                                  a[1, 0] * vector.x + a[1, 1] * vector.y + a[1, 2] * vector.z,
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

    public Mat3 transposed()
    {
        Mat3 newMatrix = new Mat3(new float[,]
        {   {matrix[0,0], matrix[1,0], matrix[2, 0] },
            {matrix[0,1], matrix[1,1], matrix[2, 1] },
            {matrix[0,2], matrix[1,2], matrix[2, 2] },
        });
        return newMatrix;
    }

    public Vec3 transform(Vec3 v)
    {
        return this * v;
    }

    public Vec3 transformTranspose(Vec3 v)
    {
        return this.transposed() * v;
    }

    public void setComponents(Vec3 v1, Vec3 v2, Vec3 v3)
    {
        matrix[0, 0] = v1.x;
        matrix[0, 1] = v1.y;
        matrix[0, 2] = v1.z;

        matrix[1, 0] = v2.x;
        matrix[1, 1] = v2.y;
        matrix[1, 2] = v2.z;

        matrix[2, 0] = v3.x;
        matrix[2, 1] = v3.y;
        matrix[2, 2] = v3.z;
    }
}
