using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FABRIK : MonoBehaviour
{

    public Transform[] joints;
    public Transform target;

    private Vec3[] copy;
    private float[] distances;
    private bool done;

    public float maxIterations = 10;

    public float treshold_condition = 0.01f;

    // Use this for initialization
    void Start()
    {
        distances = new float[joints.Length - 1];
        copy = new Vec3[joints.Length];
    }

    // Update is called once per frame
    void Update()
    {

        Vec3 targetPosition = (Vec3)target.position;
        copy[0] = (Vec3)joints[0].position;
        for (int i = 0; i < copy.Length - 1; i++)
        {
            copy[i + 1] =(Vec3) joints[i + 1].position;
            distances[i] = Vec3.Distance(copy[i + 1], copy[i]);
        }

        done = (copy[copy.Length - 1] - targetPosition).magnitude() < treshold_condition;
        if (!done)
        {
            float targetRootDist = Vec3.Distance(copy[0], targetPosition);

            // Update joint positions
            if (targetRootDist > distances.Sum())
            {
                // The target is unreachable
                //TODO3

                for (int i = 0; i < copy.Length - 1; i++)
                {
                    float r = Vec3.Distance(targetPosition, copy[i]);
                    float lambda = distances[i] / r;
                    copy[i + 1] = (1 - lambda) * copy[i] + (lambda * targetPosition);

                }
            }
            else
            {

                float comvulguis = Vec3.Distance(targetPosition, copy[copy.Length - 1]);
                Vec3 b = copy[0];

                int iter = 0;
                // The target is reachable
                while (comvulguis > treshold_condition && iter < maxIterations)
                {


                    iter++;
                    // STAGE 1: FORWARD REACHING
                    //TODO

                    copy[copy.Length - 1] = targetPosition;
                    for (int i = copy.Length - 2; i > 0; i--)
                    {
                        float r = Vec3.Distance(copy[i + 1], copy[i]);
                        float lambda = distances[i] / r;
                        copy[i] = (1 - lambda) * copy[i + 1] + (lambda * copy[i]);


                    }
                    // STAGE 2: BACKWARD REACHING
                    //TODO
                    copy[0] = b;
                    for (int i = 0; i < copy.Length - 1; i++)
                    {
                        float r = Vec3.Distance(copy[i + 1], copy[i]);
                        float lambda = distances[i] / r;
                        copy[i + 1] = (1 - lambda) * copy[i] + (lambda * copy[i + 1]);

                    }
                    comvulguis = Vec3.Distance(copy[copy.Length - 1], targetPosition);


                }
            }

            // Update original joint rotations
            for (int i = 0; i <= joints.Length - 2; i++)
            {
                //TODO4 
                //without rotations of the different pieces:
                //joints[i + 1].position = copy[i + 1];
                //with rotations of the different pieces:
                Vec3 init = (Vec3)(joints[i + 1].position - joints[i].position);
                Vec3 now = copy[i + 1] - copy[i];
                if (init != now)
                {
                    //float angle = Mathf.Acos(Vector3.Dot(init.normalized, now.normalized))*Mathf.Rad2Deg;
                    float cosa = Vec3.dotProduct(init.normalized(), now.normalized());
                    float sina = Vec3.crossProduct(init.normalized(), now.normalized()).magnitude();

                    float angle = Mathf.Atan2(sina, cosa);


                    Vec3 axis = Vec3.crossProduct(init, now);
                    axis.normalize();
                    //joints[i].rotation = (Quaternion)(new Quat(axis,angle) * (Quat)joints[i].rotation);

                    // joints[i].rotation = Quaternion.AngleAxis(angle*Mathf.Rad2Deg, (Vector3)axis) * joints[i].rotation;
                    joints[i].rotation = (Quaternion)(new Quat(axis, angle) * (Quat)joints[i].rotation);

                    joints[i + 1].position = (Vector3)copy[i + 1];
                }
            }
        }
    }
}
