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
            //SolveCollision(collisions[i]);
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
            -distance);
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
            sphere1.GetRadius() + sphere2.GetRadius() - distance);
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
                CollisionData data = new CollisionData(plane.GetNormal() * (vertexDistance - plane.GetOffset()) + vertex,
                    plane.GetNormal(),
                    plane.GetOffset() - vertexDistance);
                collisions.Add(data);
            }
        }
    }
    void CollisionBoxBox(Box box1, Box box2)
    {

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

        CollisionData data = new CollisionData((sphere.position - closestBoxPoint).normalized(), closestBoxPoint, sphere.GetRadius() - Mathf.Sqrt(dist));

    }
    void SolveCollision(CollisionData data)
    {
        PhysicalObject A = new Sphere();
        PhysicalObject B = new Sphere();
        Vec3 pointA = A.GetVelocity() + Vec3.crossProduct(A.angularVelocity, data.GetContactPoint() - A.GetPosition());
        Vec3 pointB = B.GetVelocity() + Vec3.crossProduct(B.angularVelocity, data.GetContactPoint() - B.GetPosition());

        float relV = Vec3.dotProduct(data.GetContactNormal(), (pointA - pointB));
        //Early exit
        if (relV >= 0)
            return;

        float firstPart = Vec3.dotProduct(data.GetContactNormal(), Vec3.crossProduct(A.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), data.GetContactPoint()));
        float secondPart = Vec3.dotProduct(data.GetContactNormal(), Vec3.crossProduct(B.inverseInertiaTensor * Vec3.crossProduct(data.GetContactPoint(), data.GetContactNormal()), data.GetContactPoint()));
        float factor = A.GetInverseMass() + B.GetInverseMass() + firstPart + secondPart;
        float j = -(1 + 1) * relV / factor;

        Vec3 impulse = j * data.GetContactNormal();
        A.AddForce(impulse, data.GetContactPoint() - A.GetPosition());
        B.AddForce(impulse, data.GetContactPoint() - B.GetPosition());
    }
}
