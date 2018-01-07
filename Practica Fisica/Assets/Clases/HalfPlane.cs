using UnityEngine;
using System.Collections;
using System;

public class HalfPlane : PhysicalObject
{

    //Half planes are like walls, they don't move and have infinite mass
    [SerializeField]
    private float offset;
    [SerializeField]
    private Vector3 displayNormal;
    private Vec3 normal;

    public override void Start()
    {
        bodyType = BodyType.Type_Plane;
        normal = (Vec3)displayNormal.normalized;
        inverseMass = 0;
        mass = 0;
    }
    public float GetOffset() { return offset; }
    public Vec3 GetNormal() { return normal; }
    public void SetOffset(float _offset) { offset = _offset; }
    public void SetNormal(Vec3 _normal) { normal = _normal; }

    public override void Update()
    {
        
    }
    public override void CustomUpdate()
    {
        
    }
}