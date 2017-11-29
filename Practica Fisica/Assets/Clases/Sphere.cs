using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour{

    [SerializeField]
    private Vec3 position;
    private Vec3 velocity;
    private Vec3 linearMomentum;
    private Vec3 gravity;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float mass;
    private Mat3 inertiaTensor;

    public void Start()
    {
        inertiaTensor = new Mat3( new float[,]{ { (2/5)*mass*radius*radius, 0, 0},
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
        float dt = Time.deltaTime;
        //Update the linear momentum for the gravity force which is applied each frame
        linearMomentum = linearMomentum + (gravity * dt);
        velocity = linearMomentum / mass;
        position = position + velocity * dt;

        this.transform.position = new Vector3(position.x, position.y, position.z);
    }
    public void AddForce(Vec3 force)
    {
        linearMomentum = linearMomentum + force; // no dt because it's an impulse force
    }
}
