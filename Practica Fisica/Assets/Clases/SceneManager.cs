using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    private List<PhysicalObject> objects;
    private List<CollisionData> collisions;

    void Start()
    {
        objects = new List<PhysicalObject>();
        collisions = new List<CollisionData>();
        PhysicalObject[] bodies = UnityEngine.Object.FindObjectsOfType<PhysicalObject>();
        for (int i = 0; i < bodies.Length; i++)
        {
            objects.Add(bodies[i]);
        }
    }

        void Update () {
        //Update the objects
		for(int i = 0; i < objects.Count; i++)
        {
            objects[i].CustomUpdate();
        }
        //Make them collide
        for (int i = 0; i < objects.Count; i++)
        {
            //We won't iterate the last object since all collisions have been already solved
            if(i != objects.Count - 1)
                for(int j = i + 1; j < objects.Count; j++)
                {
                    switch (objects[i].bodyType)
                    {
                        case BodyType.Type_Box:
                            switch (objects[j].bodyType)
                            {
                                case BodyType.Type_Box:
                                    CollisionBoxBox((Box)objects[i], (Box)objects[j]);
                                    break;
                                case BodyType.Type_Sphere:
                                    CollisionBoxSphere((Box)objects[i], (Sphere)objects[j]);
                                    break;
                                case BodyType.Type_Plane:
                                    CollisionBoxHalfPlane((Box)objects[i], (HalfPlane)objects[j]);
                                    break;
                            }
                            break;
                        case BodyType.Type_Sphere:
                            switch (objects[j].bodyType)
                            {
                                case BodyType.Type_Box:
                                    CollisionBoxSphere((Box)objects[j], (Sphere)objects[i]);
                                    break;
                                case BodyType.Type_Sphere:
                                    CollisionSphereSphere((Sphere)objects[i], (Sphere)objects[j]);
                                    break;
                                case BodyType.Type_Plane:
                                    CollisionSphereHalfPlane((Sphere)objects[i], (HalfPlane)objects[j]);
                                    break;
                            }
                            break;
                        case BodyType.Type_Plane:
                            switch (objects[j].bodyType)
                            {
                                case BodyType.Type_Box:
                                    CollisionBoxHalfPlane((Box)objects[j], (HalfPlane)objects[i]);
                                    break;
                                case BodyType.Type_Sphere:
                                    CollisionSphereHalfPlane((Sphere)objects[j], (HalfPlane)objects[i]);
                                    break;
                            }
                            break;
                    }
                }
        }
        //Solve the collisions
        for (int i = 0; i < collisions.Count; i++)
        {
            SolveCollision(collisions[i]);
        }
        collisions.Clear();
	}

    void CollisionSphereHalfPlane(Sphere sphere, HalfPlane plane)
    {
        float distance = Vec3.dotProduct(plane.GetNormal(), sphere.GetPosition()) - sphere.GetRadius() - plane.GetOffset();
        if (distance >= 0)
            return;
        CollisionData data = new CollisionData(plane.GetNormal(),
            sphere.GetPosition() - plane.GetNormal() * (distance + sphere.GetRadius()),
            -distance,
            sphere,
            plane
            );
        collisions.Add(data);
    }
    void CollisionSphereSphere(Sphere sphere1, Sphere sphere2)
    {
        Vec3 midPoint = sphere1.GetPosition() - sphere2.GetPosition();
        float distance = midPoint.magnitude();
        //Early exit;
        if(distance > sphere1.GetRadius() + sphere2.GetRadius())
        {
            return;
        }
        CollisionData data = new CollisionData(midPoint.normalized(),
            sphere2.GetPosition() + midPoint / 2f,
            sphere1.GetRadius() + sphere2.GetRadius() - distance,
            sphere1,
            sphere2
            );
        collisions.Add(data);
    }
    void CollisionBoxHalfPlane(Box box, HalfPlane plane)
    {
        Vec3[] boxVertices = box.GetWorldVertices();
        for (int i = 0; i < boxVertices.Length; i++)
        {
            Vec3 vertex = boxVertices[i];
            float vertexDistance = Vec3.dotProduct(vertex, plane.GetNormal());
            if (vertexDistance <= plane.GetOffset())
            {
                CollisionData data = new CollisionData(plane.GetNormal(),
                    plane.GetNormal() * (vertexDistance - plane.GetOffset()) + vertex,
                    plane.GetOffset() - vertexDistance,
                    box,
                    plane
                    );
                collisions.Add(data);
            }
        }
    }
    void CollisionBoxBox(Box box1, Box box2)
    {
        //earlyOutTest
        Vec3[] axis = new Vec3[15];
        axis[0] = box1.getAxisX();
        axis[1] = box1.getAxisY();
        axis[2] = box1.getAxisZ();

        axis[3] = box2.getAxisX();
        axis[4] = box2.getAxisY();
        axis[5] = box2.getAxisZ();

        axis[6] = Vec3.crossProduct(box1.getAxisX(), box2.getAxisX());
        axis[7] = Vec3.crossProduct(box1.getAxisX(), box2.getAxisY());
        axis[8] = Vec3.crossProduct(box1.getAxisX(), box2.getAxisZ());

        axis[9] = Vec3.crossProduct(box1.getAxisY(), box2.getAxisX());
        axis[10] = Vec3.crossProduct(box1.getAxisY(), box2.getAxisY());
        axis[11] = Vec3.crossProduct(box1.getAxisY(), box2.getAxisZ());

        axis[12] = Vec3.crossProduct(box1.getAxisZ(), box2.getAxisX());
        axis[13] = Vec3.crossProduct(box1.getAxisZ(), box2.getAxisY());
        axis[14] = Vec3.crossProduct(box1.getAxisZ(), box2.getAxisZ());

        for (int i = 0; i < 15; i++)
        {
            if (!overlapOnAxis(box1, box2, axis[i])) return;
        }

        //point Face Contact

        Vec3[] box1Vertices = box1.GetWorldVertices();
        float depestPen = CollisionBoxPoint(box2, box1Vertices[0]);
        float newPen;
        for (int i = 1; i < box1Vertices.Length; i++)
        {
            //newPen=CollisionBoxPoint(box)
        }


    }

    float CollisionBoxPoint(Box box, Vec3 point)
    {
        Vec3 relPos = box.getLocalCoordinates(point);
        Vec3 normal;

        float minDepth = box.GetHalfSize().x - Mathf.Abs(relPos.x);
        if (minDepth < 0) return 0;//si el punto esta fuera salimos
        normal = box.getAxisX() * ((relPos.x < 0) ? -1 : 1);

        float depth = box.GetHalfSize().y - Mathf.Abs(relPos.y);
        if (depth < 0) return 0;
        else if (depth < minDepth)
        {
            minDepth = depth;
            normal = box.getAxisY() * ((relPos.y < 0) ? -1 : 1);
        }
        depth = box.GetHalfSize().z - Mathf.Abs(relPos.z);
        if (depth < 0) return 0;
        else if (depth < minDepth)
        {
            minDepth = depth;
            normal = box.getAxisZ() * ((relPos.z < 0) ? -1 : 1);
        }
        return minDepth;

    }
    bool overlapOnAxis(Box box1, Box box2, Vec3 axis)
    {
        float oneProject = box1.transformToAxis(axis);
        float twoProject = box2.transformToAxis(axis);

        Vec3 toCenter = box2.position - box1.position;

        float distance = Mathf.Abs(Vec3.dotProduct(toCenter, axis));
        return (distance < oneProject + twoProject);

    }
    void CollisionBoxSphere(Box box, Sphere sphere)
    {
        //convertimos la posicion de la esfera a coordenadas locales del cubo
        Vec3 localSpherePos = box.getLocalCoordinates(sphere.position);
        //Early exit: Si la esfera no esta overlapeada con el cubo en alguna de las coordenadas quiere decir que no estan colisionando
        Vec3 boxHalfSize = box.GetHalfSize();
        if (Mathf.Abs(localSpherePos.x)-sphere.GetRadius()>boxHalfSize.x ||
            Mathf.Abs(localSpherePos.y) - sphere.GetRadius() > boxHalfSize.y ||
            Mathf.Abs(localSpherePos.z) - sphere.GetRadius() > boxHalfSize.z)
        {
            return;
        }

        //ahora encontramos el punto mas cercano a la esfera en el cubo

        Vec3 closestBoxPoint=new Vec3();

        float dist = localSpherePos.x;
        if (dist > boxHalfSize.x) dist = boxHalfSize.x;
        else if (dist < -boxHalfSize.x) dist = -boxHalfSize.x;
        closestBoxPoint.x = dist;

        dist = localSpherePos.y;
        if (dist > boxHalfSize.y) dist = boxHalfSize.y;
        else if (dist < -boxHalfSize.y) dist = -boxHalfSize.y;
        closestBoxPoint.y = dist;

        dist = localSpherePos.z;
        if (dist > boxHalfSize.z) dist = boxHalfSize.z;
        else if (dist < -boxHalfSize.z) dist = -boxHalfSize.z;
        closestBoxPoint.z = dist;

        //ahora que sabemos cual es el punto mas proximo podemos comprobar si realmente se estan tocando
        dist = (closestBoxPoint - localSpherePos).squareMagintude();
        if (dist > sphere.GetRadius() * sphere.GetRadius()) return;

        //collisionData
        Quat closestBoxPointQuat = new Quat(closestBoxPoint.x, closestBoxPoint.y, closestBoxPoint.z, 0);
        closestBoxPointQuat = box.rotation * closestBoxPointQuat * box.rotation.conjugated();

        closestBoxPoint.x = closestBoxPointQuat.x;
        closestBoxPoint.y = closestBoxPointQuat.y;
        closestBoxPoint.z = closestBoxPointQuat.z;

        CollisionData data = new CollisionData((sphere.position - closestBoxPoint).normalized(),
            closestBoxPoint, 
            sphere.GetRadius() - Mathf.Sqrt(dist),
            box,
            sphere);

        collisions.Add(data);

    }
    void SolveCollision(CollisionData data)
    {
        PhysicalObject A = data.objects[0];
        PhysicalObject B = data.objects[1];
        PhysicalObject nonPlane = null;
        if (A.bodyType == BodyType.Type_Plane)
        {
            nonPlane = B;
        }
        else if (B.bodyType == BodyType.Type_Plane)
        {
            nonPlane = A;
        }
        if(nonPlane != null)
        {
            float relV = Vec3.dotProduct(data.GetContactNormal(), nonPlane.GetVelocity());
            if (relV >= 0f)
                return;
            /*
            float firstPart = Vec3.dotProduct(data.GetContactNormal(), Vec3.crossProduct(nonPlane.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), data.GetContactPoint()));
            float factor = nonPlane.GetInverseMass() + firstPart;
            float j = -(1 + 1) * relV / factor;
            */
            float topPart = Vec3.dotProduct(nonPlane.velocity, data.GetContactNormal()) + Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), nonPlane.angularVelocity);
            float part1 = Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), nonPlane.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()));
            float bottomPart = nonPlane.GetInverseMass() + part1;
            float j = -(1 + 1) * topPart / bottomPart;

            Vec3 impulse = j * data.GetContactNormal();
            Debug.DrawLine((Vector3)nonPlane.GetPosition(), (Vector3)data.GetContactPoint());
            nonPlane.SetPosition(nonPlane.GetPosition() + data.GetContactNormal() * data.GetPenetrationDepth());
            if (relV < 0.001f)
                nonPlane.AddForce(impulse, nonPlane.getLocalCoordinates(data.GetContactPoint()));
        }
        else
        {
            Vec3 pointA = A.GetVelocity() + Vec3.crossProduct(A.angularVelocity, data.GetContactPoint() - A.GetPosition());
            Vec3 pointB = B.GetVelocity() + Vec3.crossProduct(B.angularVelocity, data.GetContactPoint() - B.GetPosition());

            float relV = Vec3.dotProduct(data.GetContactNormal(), (pointA - pointB));
            //Early exit
            if (relV > 0)
                return;
            /*
            float firstPart = Vec3.dotProduct(data.GetContactNormal(), Vec3.crossProduct(A.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), data.GetContactPoint()));
            float secondPart = Vec3.dotProduct(data.GetContactNormal(), Vec3.crossProduct(B.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), data.GetContactPoint()));
            float factor = A.GetInverseMass() + B.GetInverseMass() + firstPart + secondPart;
            float j = -(1 + 1) * relV / factor;
            */
            float topPart = Vec3.dotProduct(A.velocity - B.velocity, data.GetContactNormal()) + Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), A.angularVelocity) + Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), B.angularVelocity);
            float part1 = Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), A.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()));
            float part2 = Vec3.dotProduct(Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), B.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()));
            float bottomPart = A.GetInverseMass() + B.GetInverseMass() + part1 + part2;
            float j = -(1 + 1) * topPart / bottomPart;
            

            Vec3 impulse = j * data.GetContactNormal();
            Vec3 impulseB = new Vec3(-impulse.x, -impulse.y, -impulse.z);
            A.AddForce(impulse * A.GetVelocity().magnitude(), A.getLocalCoordinates(data.GetContactPoint()));
            B.AddForce(impulseB * B.GetVelocity().magnitude(), B.getLocalCoordinates(data.GetContactPoint()));
        }


    }
}
