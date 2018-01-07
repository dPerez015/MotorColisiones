using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyType {Type_Sphere, Type_Box, Type_Plane };

public abstract class PhysicalObject : MonoBehaviour
{
    //Movement variables
    public Vec3 position;
    public Vec3 velocity;
    public Vec3 linearMomentum;

    //Rotation Variables
    public Quat rotation;
    public Vec3 torque;
    public Vec3 angularMomentum;
    public Vec3 angularVelocity;

    //World Variables
    public Vec3 gravity;

    public float mass;
    //Eis
    public float inverseMass;

    public BodyType bodyType;

    //Inertia tensor
    public Mat3 inertiaTensor;
    public Mat3 inverseInertiaTensor;

    //Methods
    public abstract void Start();

    public Vec3 GetPosition() { return position; }
    public Vec3 GetVelocity() { return velocity; }
    public float GetMass() { return mass; }
    public float GetInverseMass() { return inverseMass; }
    public void SetPosition(Vec3 pos) { position = pos; }
    public virtual void SetMass(float m)
    {
        mass = m;
        if (mass != 0)
            inverseMass = 1 / mass;
        else
            inverseMass = 0;
    }

    public virtual void CustomUpdate()
    {
        EulerStep(Time.deltaTime);
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Space Pressed");
            AddForce(new Vec3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), new Vec3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
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
    public abstract void Update();
    public void Initialisation()
    {
        position = (Vec3)this.transform.position;
        velocity = new Vec3();
        linearMomentum = new Vec3(0f, 10f, 0f);

        gravity = new Vec3(0f, -9.81f, 0f);

        angularMomentum = new Vec3(0f, 0f, 0f);
        angularVelocity = new Vec3();
        torque = new Vec3();
        rotation = (Quat)this.transform.rotation;
        if (mass != 0)
        {
            inverseMass = 1 / mass;
        }
        else
        {
            inverseMass = 0;
        }
    }

    public Vec3 getLocalCoordinates(Vec3 original)
    {
        Vec3 ret = original - position;
        Quat retQuat = new Quat(ret.x, ret.y, ret.z, 0);
        retQuat = rotation.conjugated() * retQuat * rotation;

        ret.x = retQuat.x;
        ret.y = retQuat.y;
        ret.z = retQuat.z;
        return ret;
    }

}
