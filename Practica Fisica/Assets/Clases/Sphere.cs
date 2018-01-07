using UnityEngine;
using System.Collections;

public class Sphere : PhysicalObject
{
    private float radius;

    public override void Start()
    {
        Initialisation();
        bodyType = BodyType.Type_Sphere;
        radius = this.transform.localScale.x;
        inertiaTensor = new Mat3(new float[,]{ { (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, (2f/5f)*mass*radius*radius} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
    }
    public float GetRadius() { return radius; }
    public void SetRadius(float r)
    {
        radius = r;
        inertiaTensor = new Mat3(new float[,]{ { (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, (2f/5f)*mass*radius*radius} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
    }
    public override void SetMass(float m)
    {
        SetMass(m);

        inertiaTensor = new Mat3(new float[,]{ { (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, (2f/5f)*mass*radius*radius} });

        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
    }
    public override void Update()
    {
        //Update the object's position
       // CustomUpdate();
    }
}

