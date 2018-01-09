using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDataBox{

    public struct data
    {
        public data(Vec3 norm, Vec3 contact,int inx,  float pen, PhysicalObject o)
        {
            penetration = pen;
            normal = norm;
            contactPoint = contact;
            vertexIndex = inx;
            obj = o;
        }
        public float penetration;
        public Vec3 normal;
        public Vec3 contactPoint;
        public int vertexIndex; 
        public PhysicalObject obj;
    }
    public List<data> contacts;
    public Box box;

    public CollisionDataBox(PhysicalObject b)
    {
        contacts =new List<data>();
        box = (Box)b;
    } 
}
