using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : PhysicalObject
{
    struct contact
    {

        float penetration;

        // public contact()
    }
    //enum TypeOfContact{,c }

    //Object variables
    private Vec3 size;
    private Vec3 halfSize;
    private Vec3[] vertices;
    private Vec3[] WorldVertices;
    private List<float> contacts;

    public override void Start()
    {
        Initialisation();
        //contacts=new List<>
        bodyType = BodyType.Type_Box;
        size = (Vec3)this.transform.localScale;
        halfSize = size / 2f;

        Vec3 pos = (Vec3)this.transform.position;

        vertices = new Vec3[] {
            new Vec3(- halfSize.x, + halfSize.y, + halfSize.z),
            new Vec3(- halfSize.x, + halfSize.y, - halfSize.z),
            new Vec3(+ halfSize.x, + halfSize.y, + halfSize.z),
            new Vec3(+ halfSize.x, + halfSize.y, - halfSize.z),
            new Vec3(- halfSize.x, - halfSize.y, + halfSize.z),
            new Vec3(- halfSize.x, - halfSize.y, - halfSize.z),
            new Vec3(+ halfSize.x, - halfSize.y, + halfSize.z),
            new Vec3(+ halfSize.x, - halfSize.y, - halfSize.z)};

        WorldVertices = new Vec3[] {
            new Vec3(- halfSize.x, + halfSize.y, + halfSize.z),
            new Vec3(- halfSize.x, + halfSize.y, - halfSize.z),
            new Vec3(+ halfSize.x, + halfSize.y, + halfSize.z),
            new Vec3(+ halfSize.x, + halfSize.y, - halfSize.z),
            new Vec3(- halfSize.x, - halfSize.y, + halfSize.z),
            new Vec3(- halfSize.x, - halfSize.y, - halfSize.z),
            new Vec3(+ halfSize.x, - halfSize.y, + halfSize.z),
            new Vec3(+ halfSize.x, - halfSize.y, - halfSize.z)};

        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });

    }
    public override void Reset()
    {
        base.Reset();
        updateVertices();
    }
    public Vec3 GetSize() { return size; }
    public Vec3 GetHalfSize() { return halfSize; }
    public Vec3[] GetWorldVertices() { return WorldVertices; }
    public void SetSize(Vec3 _size)
    {
        size = _size;
        halfSize = size / 2f;
        this.transform.localScale = new Vector3(size.x, size.y, size.z);
        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        Vec3 pos = (Vec3)this.transform.position;
        vertices = new Vec3[] {
            new Vec3(pos.x - halfSize.x, pos.y + halfSize.y, pos.z + halfSize.z),
            new Vec3(pos.x - halfSize.x, pos.y + halfSize.y, pos.z - halfSize.z),
            new Vec3(pos.x + halfSize.x, pos.y + halfSize.y, pos.z + halfSize.z),
            new Vec3(pos.x + halfSize.x, pos.y + halfSize.y, pos.z - halfSize.z),
            new Vec3(pos.x - halfSize.x, pos.y - halfSize.y, pos.z + halfSize.z),
            new Vec3(pos.x - halfSize.x, pos.y - halfSize.y, pos.z - halfSize.z),
            new Vec3(pos.x + halfSize.x, pos.y - halfSize.y, pos.z + halfSize.z),
            new Vec3(pos.x + halfSize.x, pos.y - halfSize.y, pos.z - halfSize.z)};
    }
    public override void SetMass(float m)
    {
        SetMass(m);
        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
    }
    public override void Update()
    {
        //Update the object's position
        // CustomUpdate();
    }
    public override void CustomUpdate()
    {
        base.CustomUpdate();
        //Update the vertices
        for (int i = 0; i < WorldVertices.Length; i++)
        {
            Quat vertexQuat = new Quat(vertices[i].x, vertices[i].y, vertices[i].z, 0);
            Quat newVertex = rotation * vertexQuat * rotation.conjugated();
            WorldVertices[i] = (Vec3)this.transform.position + new Vec3(newVertex.x, newVertex.y, newVertex.z);
        }
    }
    public void updateVertices()
    {
        for (int i = 0; i < WorldVertices.Length; i++)
        {
            Quat vertexQuat = new Quat(vertices[i].x, vertices[i].y, vertices[i].z, 0);
            Quat newVertex = rotation * vertexQuat * rotation.conjugated();
            WorldVertices[i] = (Vec3)this.transform.position + new Vec3(newVertex.x, newVertex.y, newVertex.z);
        }

    }
    public float transformToAxis(Vec3 axis)
    {
        return halfSize.x * Mathf.Abs(Vec3.dotProduct(axis, getAxisX())) +
            halfSize.y * Mathf.Abs(Vec3.dotProduct(axis, getAxisY())) +
            halfSize.z * Mathf.Abs(Vec3.dotProduct(axis, getAxisZ()));
        
    }
}
