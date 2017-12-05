using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

    //Movement variables
    [SerializeField]
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
    private float radius;
    [SerializeField]
    private float mass;

    //Inertia tensor
    private Mat3 inertiaTensor;
    private Mat3 inverseInertiaTensor;

    public void Start()
    {
        inertiaTensor = new Mat3(new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
                                                { 0, (2/5)*mass*radius*radius, 0},
                                                { 0, 0, (2/5)*mass*radius*radius} });
        position = new Vec3();
        velocity = new Vec3();
        gravity = new Vec3(0f, -9.81f, 0f);
        linearMomentum = new Vec3(0f, 10f, 0f);

    }
    public Vec3 GetPosition() { return position; }
    public Vec3 GetVelocity() { return velocity; }
    public float GetRadius() { return radius; }
    public float GetMass() { return mass; }
    public void SetPosition(Vec3 pos) { position = pos; }
    public void SetRadius(float r) {
        radius = r;
        inertiaTensor = new Mat3(new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
                                                { 0, (2/5)*mass*radius*radius, 0},
                                                { 0, 0, (2/5)*mass*radius*radius} });
    }
    public void SetMass(float m) {
        mass = m;
        inertiaTensor = new Mat3(new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
                                                { 0, (2/5)*mass*radius*radius, 0},
                                                { 0, 0, (2/5)*mass*radius*radius} });
    }
    public void Update()
    {

        //Update the object's position
        EulerStep(Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Space Pressed");
            AddForce(new Vec3(0f, 10f, 0f), new Vec3(0.25f, 0f, 0f));
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
        Mat3 tempI = rotation.quatToMat() * inverseInertiaTensor * rotation.conjugated().quatToMat();
        angularVelocity = tempI * (angularMomentum * dt);
        Quat tempW = new Quat( 0.5f * dt * angularVelocity, 0);
        rotation = tempW * rotation;
        rotation.normalize();
        
    }
}
