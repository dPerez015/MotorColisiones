using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{

    //Movement variables
    private Vec3 position;
    private Vec3 velocity;
    private Vec3 linearMomentum;

    //Rotation Variables
    private Quat rotation;
    private Vec3 torque;
    private Vec3 angularMomentum;
    private Vec3 angularVelocity;

    //World Variables
    private Vec3 gravity;

    //Object variables
    [SerializeField]
    private Vec3 size;

    [SerializeField]
    private float mass;
    private float inverseMass;

    //Inertia tensor
    private Mat3 inertiaTensor;
    private Mat3 inverseInertiaTensor;

    public void Start()
    {
        size = (Vec3)this.transform.localScale;

        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
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
    public Vec3 GetPosition() { return position; }
    public Vec3 GetVelocity() { return velocity; }
    public Vec3 GetSize() { return size; }
    public float GetMass() { return mass; }
    public float GetInverseMass() { return inverseMass; }
    public void SetPosition(Vec3 pos) { position = pos; }
    public void SetSize(Vec3 _size)
    {
        size = _size;
        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
    }
    public void SetMass(float m)
    {
        mass = m;
        if (mass != 0)
            inverseMass = 1 / mass;
        else
            inverseMass = 0;

        inertiaTensor = new Mat3(new float[,]{ { (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
        inverseInertiaTensor = new Mat3(new float[,]{ { 1f / (1f/12f)*mass*Mathf.Pow(size.y, 2)*Mathf.Pow(size.z, 2), 0, 0},
                                                { 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.z, 2), 0},
                                                { 0, 0, 1f / (1f/12f)*mass*Mathf.Pow(size.x, 2)*Mathf.Pow(size.y, 2)} });
    }
    public void Update()
    {

        //Update the object's position
        this.EulerStep(Time.deltaTime);

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
}
