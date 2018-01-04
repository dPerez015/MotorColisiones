﻿using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour
{

    //Movement variables
    [SerializeField]
    private Vec3 position;
    private Vec3 velocity;
    private Vec3 linearMomentum;

    //Rotation Variables
    [SerializeField]
    private Quat rotation;
    [SerializeField]
    private Vec3 torque;
    [SerializeField]
    private Vec3 angularMomentum;
    [SerializeField]
    private Vec3 angularVelocity;

    //World Variables
    private Vec3 gravity;

    //Object variables
    [SerializeField]
    private float radius;
    [SerializeField]
    private float mass;
    private float inverseMass;

    //Inertia tensor
    private Mat3 inertiaTensor;
    private Mat3 inverseInertiaTensor;

    public void Start()
    {
        inertiaTensor = new Mat3(new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
                                                { 0, (2/5)*mass*radius*radius, 0},
                                                { 0, 0, (2/5)*mass*radius*radius} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
        position = (Vec3)this.transform.position;
        velocity = new Vec3();
        linearMomentum = new Vec3(0f, 10f, 0f);

        gravity = new Vec3(0f, -9.81f, 0f);

        angularMomentum = new Vec3(0f, 0f, 0f);
        angularVelocity = new Vec3();
        torque = new Vec3();
        rotation = (Quat)this.transform.rotation;
        if(mass != 0)
        {
            inverseMass = 1 / mass;
        } else
        {
            inverseMass = 0;
        }

    }
    public Vec3 GetPosition() { return position; }
    public Vec3 GetVelocity() { return velocity; }
    public float GetRadius() { return radius; }
    public float GetMass() { return mass; }
    public float GetInverseMass() { return inverseMass; }
    public void SetPosition(Vec3 pos) { position = pos; }
    public void SetRadius(float r)
    {
        radius = r;
        inertiaTensor = new Mat3(new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
                                                { 0, (2/5)*mass*radius*radius, 0},
                                                { 0, 0, (2/5)*mass*radius*radius} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
    }
    public void SetMass(float m)
    {
        mass = m;
        if (mass != 0)
            inverseMass = 1 / mass;
        else
            inverseMass = 0;

        inertiaTensor = new Mat3(new float[,]{ { (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, (2f/5f)*mass*radius*radius} });

        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (2f/5f)*mass*radius*radius, 0, 0},
                                                { 0, 1f / (2f/5f)*mass*radius*radius, 0},
                                                { 0, 0, 1f / (2f/5f)*mass*radius*radius} });
    }
    public void Update()
    {

        //Update the object's position
        this.EulerStep(Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Space Pressed");
            AddForce(new Vec3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), new Vec3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            //AddForce(new Vec3(10f, 10f, 10f), new Vec3(1f, 0f, 0f));
        }
    }
    public void AddForce(Vec3 force, Vec3 applicationPoint)
    {
        //The application point is expressed in the sphere local coordinates
        linearMomentum += force; // no dt because it's an impulse force
        torque = Vec3.crossProduct(force, applicationPoint);
        angularMomentum += torque;
    }

    public void EulerStep(float dt)
    {
        //The object is moved here
        linearMomentum += (gravity * mass * dt);
        velocity = linearMomentum / mass;
        position += velocity * dt;

        this.transform.position = new Vector3(position.x, position.y, position.z);

        //The object is rotated here
        Mat3 rotationMatrix = rotation.quatToMat();
        Mat3 tempI = rotationMatrix * inverseInertiaTensor * rotationMatrix.transposed();
        angularVelocity = tempI * (angularMomentum * dt);
        if (angularVelocity.magnitude() != 0f)
        {
            rotation = new Quat(angularVelocity, angularVelocity.magnitude()) * rotation;
            rotation.normalize();
            this.transform.rotation = (Quaternion)rotation;
        }
    }
}

