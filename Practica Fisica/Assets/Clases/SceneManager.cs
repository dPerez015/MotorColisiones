using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    private List<PhysicalObject> objects;
    private List<CollisionData> collisions;
    void Start () {
        objects = new List<PhysicalObject>();
        collisions = new List<CollisionData>();
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

    }
    void CollisionBoxBox(Box box1, Box box2)
    {

    }
    void CollisionBoxSphere(Box box, Sphere sphere)
    {

    }
}
