using UnityEngine;
using System.Collections;

public class Box : PhysicalObject
{


    //Object variables
    private Vec3 size;

    public override void Start()
    {
        Initialisation();
        bodyType = BodyType.Type_Box;
        size = (Vec3)this.transform.localScale;

        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });

    }
    public Vec3 GetSize() { return size; }
    public void SetSize(Vec3 _size)
    {
        size = _size;
        this.transform.localScale = new Vector3(size.x, size.y, size.z);
        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
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
        CustomUpdate();
    }
}
