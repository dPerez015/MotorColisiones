using UnityEngine;
using System.Collections;

public class CollisionData
{
    private Vec3 contact_normal;
    private Vec3 contact_point;
    private float penetration_depth;
    public PhysicalObject[] objects;
    public Mat3 contactToWorld;

    private Vec3[] contactTangent;

    public CollisionData (Vec3 normal, Vec3 point, float depth, PhysicalObject ob1, PhysicalObject ob2)
    {
        contact_normal = normal;
        contact_point = point;
        penetration_depth = depth;
        objects = new PhysicalObject[2];
        objects[0] = ob1;
        objects[1] = ob2;
        contactToWorld = new Mat3();
        contactTangent = new Vec3[2];
    }
    public Vec3 GetContactNormal() { return contact_normal; }
    public Vec3 GetContactPoint() { return contact_point; }
    public float GetPenetrationDepth() { return penetration_depth; }

    public void SetContactNormal(Vec3 normal) { contact_normal = normal; }
    public void SetContactPoint(Vec3 point) { contact_point = point; }
    public void SetPenetrationDepth(float depth) { penetration_depth = depth; }

    public void calculateContactBasis()
    {
        //comprovamos que direccion es mejor utilizar
        if (Mathf.Abs(contact_normal.x) > Mathf.Abs(contact_normal.y))
        {
            //nos aseguramos de que los resultados esten normalizados
            float scaleFactor = 1 / Mathf.Sqrt(contact_normal.z * contact_normal.z + contact_normal.x * contact_normal.x);

            contactTangent[0].x = contact_normal.z * scaleFactor;
            contactTangent[0].y = 0;
            contactTangent[0].z = -contact_normal.x * scaleFactor;

            contactTangent[1].x = contact_normal.y * contactTangent[0].x;
            contactTangent[1].y = contact_normal.z * contactTangent[0].x - contact_normal.x * contactTangent[0].z;
            contactTangent[1].z = -contact_normal.y * contactTangent[0].x;
        }
        else
        {
            float scaleFactor = 1 / Mathf.Sqrt(contact_normal.z * contact_normal.z + contact_normal.y * contact_normal.y);

            contactTangent[0].x = 0;
            contactTangent[0].y = -contact_normal.z * scaleFactor;
            contactTangent[0].z = contact_normal.y * scaleFactor;

            contactTangent[1].x = contact_normal.y * contactTangent[0].z-contact_normal.z*contactTangent[0].y;
            contactTangent[1].y = -contact_normal.x * contactTangent[0].z ;
            contactTangent[1].z = contact_normal.x * contactTangent[0].y;
        }
        contactToWorld.setComponents(contact_normal, contactTangent[0], contactTangent[1]);
        
    }
}