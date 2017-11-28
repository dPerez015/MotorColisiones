using UnityEngine;
using System.Collections;

public class Vec3{
    public float x,y,z;

//constructores
    public Vec3(float _x, float _y, float _z) {
        x = _x;
        y = _y;
        z = _z;
    }
    public Vec3() {
        x = 0;
        y = 0;
        z = 0;
    }
    //vectores pre creados
    public static Vec3 ZeroVec() {
        return new Vec3();
    }
    //operadores
    public static Vec3 operator *(Vec3 v, float a)
    {
            return new Vec3(v.x * a, v.y * a, v.z * a);
    }
    public static Vec3 operator *(float a, Vec3 v)
    {
        return new Vec3(v.x * a, v.y * a, v.z * a);
    }
    public static Vec3 operator /(Vec3 v, float a) {
        if (a != 0)
            return new Vec3(v.x / a, v.y / a, v.z / a);
        else
          throw new System.Exception("Division por 0");    
    }
    public static Vec3 operator +(Vec3 a, Vec3 b) {
        return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Vec3 operator -(Vec3 a, Vec3 b)
    {
        return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static bool operator ==(Vec3 a, Vec3 b) {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return true;
        else
            return false;   
    }
    public static bool operator !=(Vec3 a, Vec3 b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return false;
        else
            return true;
    }

    //operaciones
    public static float dotProduct(Vec3 a, Vec3 b) {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    public static Vec3 crossProduct(Vec3 a, Vec3 b) {
        return new Vec3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
           );
    }

    public static float AngleBetween(Vec3 a, Vec3 b) {
        float totalSize = a.magnitude() * b.magnitude();
        if (totalSize!=0)
            return dotProduct(a, b) / totalSize;
        else
            throw new System.Exception("Vectores de tamaño 0");    
    }

    public float magnitude() {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    public Vec3 normalized() {
        if (x != 0 && y != 0 && z != 0)
            return new Vec3(this.x, this.y, this.z) / this.magnitude();
        else
            throw new System.Exception("No se pude normalizar un vec3 nulo");
    }
    public void normalize() {
        if (x != 0 && y != 0 && z != 0)
        {
            float size = this.magnitude();
            x /= size;
            y /= size;
            z /= size;
        }
        else
            throw new System.Exception("No se pude normalizar un vec3 nulo");
    }

    public static float distance(Vec3 a, Vec3 b) {
        Vec3 v = b - a;
        return v.magnitude();
    }
}
