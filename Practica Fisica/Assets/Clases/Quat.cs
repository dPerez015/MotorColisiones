﻿using UnityEngine;
using System.Collections;

public class Quat{
    public float x, y, z, w;

    public Quat(float _x, float _y, float _z, float _w) {
        x = _x;
        y = _y;
        z = _z;
        w = _w;
    }
    /*axis angle Constructor*/
    public Quat(Vec3 axis, float angle)
    {
        axis = (axis.normalized() * Mathf.Sin(angle / 2)) ;
        x = axis.x;
        y = axis.y;
        z = axis.x;
        w = Mathf.Cos(angle/2);
    }
    public Quat() {
        x = 0;
        y = 0;
        z = 0;
        w = 0;
    }


    public static Quat operator +(Quat a, Quat b) {
        return new Quat(a.x + b.x, a.y + b.y, a.z + b.z, a.z + b.z);
    }
    public static Quat operator *(Quat a, Quat b) {
        return new Quat(
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
            a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
    }
    public static Quat operator *(Quat a, float b) {
        return new Quat(a.x * b, a.y * b, a.z * b, a.w * b);
    }

    public float magnitude() {
        return Mathf.Sqrt(Mathf.Pow(w, 2) + Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    public Quat normalized() {
        if (w != 0 && x != 0 && y != 0 && z != 0)
            return this * (1 / this.magnitude());
        else
            throw new System.Exception("no se pude normalizar un Quat nulo");
    }

    public void normalize() {
        if (w != 0 && x != 0 && y != 0 && z != 0)
        {
            float mag = 1 / this.magnitude();
            w *= mag;
            x *= mag;
            y *= mag;
            z *= mag;
        }
        else
            throw new System.Exception("no se pude normalizar un Quat nulo");
    }
    public Quat conjugated() {
        return new Quat(-x, -y, -z, w);
    }
    public void conjugate() {
        x = -x;
        y = -y;
        z = -z;
    }
    public Quaternion createRotation()
    {
        return new Quaternion(x, y, z, w);

    }

}
