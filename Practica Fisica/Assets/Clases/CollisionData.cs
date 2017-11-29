using UnityEngine;
using System.Collections;

public class CollisionData
{
    private Vec3 contact_normal;
    private Vec3 contact_point;
    private float penetration_depth;

    public CollisionData (Vec3 normal, Vec3 point, float depth)
    {
        contact_normal = normal;
        contact_point = point;
        penetration_depth = depth;
    }
    public Vec3 GetContactNormal() { return contact_normal; }
    public Vec3 GetContactPoint() { return contact_point; }
    public float GetPenetrationDepth() { return penetration_depth; }
    public void SetContactNormal(Vec3 normal) { contact_normal = normal; }
    public void SetContactPoint(Vec3 point) { contact_point = point; }
    public void SetPenetrationDepth(float depth) { penetration_depth = depth; }
}