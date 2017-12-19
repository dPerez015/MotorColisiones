using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Branch {

    //public bool isLeave;
    public Transform Target;
    public Transform[] joints;
    protected Vec3[] copy;
    private float[] distances;
  //  public Transform Base;
    public Branch[] subBranches;
    protected Vec3 centroid;

    public void init(){
        copy = new Vec3[joints.Length];
        distances = new float[joints.Length - 1];
        for(int i = 0; i < subBranches.Length; i++)
        {
            subBranches[i].init();
        }
        copy[0] = (Vec3)joints[0].position;
        for (int i = 0; i < copy.Length - 1; i++)
        {
            copy[i + 1] = (Vec3)joints[i + 1].position;
            distances[i] = Vec3.Distance(copy[i], copy[i + 1]);
        }
    }

    void inverseStep(Vec3 tPos, float threshHold)
    {
        // comprovamos si es alcanzable
        float targetRootDistance = Vec3.Distance(tPos, copy[0]);
        //Es innalcancable 
        if (targetRootDistance > distances.Sum())
        {
            copy[copy.Length-1] = tPos;
            for (int i= copy.Length - 1; i > 0; i--)
            {
                float r = Vec3.Distance(copy[0], copy[i]);
                float lambda = distances[i-1] / r;
                copy[i - 1] = (1 - lambda) * copy[i] + (lambda * copy[0]);
            }

        }
        else
        {
            //es alcanzable
            copy[copy.Length - 1] = tPos;
            for(int i = copy.Length - 2; i >= 0; i--)
            {
                float r = Vec3.Distance(copy[i + 1], copy[i]);
                float lambda = distances[i] / r;
                copy[i] = (1 - lambda) * copy[i + 1] + (lambda * copy[i]); 
            }
        }
    }

    public Vec3 inverseReaching(float threshHold)
    {
        //inicializamos el vector copy y las distancias
        copy[0] = (Vec3)joints[0].position;
        for(int i = 0; i < joints.Length - 1; i++)
        {
            copy[i + 1] = (Vec3)joints[i + 1].position;
           // distances[i] = Vec3.Distance(copy[i + 1], copy[i]);
        }

        //comprovar si es rama o hoja
        if (subBranches.Length == 0)
        {
            //si es hoja hacemos el paso inverso normal
            if ((copy[copy.Length - 1] - (Vec3)Target.position).magnitude() > threshHold)
            {
                inverseStep((Vec3)Target.position, threshHold);
            }
            return copy[0];

        }
        else
        {
            //tiene subramas
            //Calculamos los inwardReaching de cada una y hacemos el centroide
            centroid = new Vec3(0, 0, 0);
            for(int i=0; i < subBranches.Length; i++)
            {
                centroid += subBranches[i].inverseReaching(threshHold);
            }
            centroid = centroid / subBranches.Length;
            inverseStep(centroid,threshHold);
            return copy[0];   
        }
    }

    public void normalReaching(Vec3 StartPos, float threshold, int maxIterations){
        //Primero calculamos sus posiciones
        //Comprovamos si es rama o hoja
        if (subBranches.Length == 0)
            fabrikAlgorithm(StartPos, (Vec3)Target.position, threshold,maxIterations);
        else
        {
            fabrikAlgorithm(StartPos, centroid, threshold,maxIterations);
           // Target.position = (Vector3)centroid;
        }
    //Despues las de sus subRamas

        for(int i = 0; i < subBranches.Length; i++)
        {
            subBranches[i].normalReaching(copy[copy.Length - 1], threshold, maxIterations);
        }


    }

    public void fabrikAlgorithm(Vec3 StartPos, Vec3 target, float threshold, int maxIterations)
    {
        //colocamos la primera parte en el punto de comienzo
        copy[0] = StartPos;
        //colocamos las copys en su sitio
        for (int i = 0; i < copy.Length-1; i++)
        {
            float r = Vec3.Distance(copy[i + 1], copy[i]);
            float lambda = distances[i] / r;
            copy[i + 1] = (1 - lambda) * copy[i] + (lambda * copy[i + 1]);
        }


        //comprovamos si ya estamos en nuestro objetivo
        if ((copy[copy.Length - 1] - target).magnitude() > threshold){
            
            //comprovamos si el target es alcanzable
            float targetRootDist = Vec3.Distance(copy[0], target);
            if (targetRootDist > distances.Sum())
            {//es inalcanzable
                for(int i = 0; i < copy.Length - 1; i++)
                {
                    float r = Vec3.Distance(target, copy[i]);
                    float lambda = distances[i] / r;
                    copy[i + 1] = (1 - lambda) * copy[i] + (lambda * target);
                }
            }
            else
            {//es alcanzable
                float targetEndEDistance = Vec3.Distance(target,copy[copy.Length-1]);
                int iter = 0;
                while(targetEndEDistance>threshold && iter < maxIterations)
                {
                    iter++;
                    //forward Reaching
                    copy[copy.Length - 1] = target;
                    for(int i = copy.Length - 2; i > 0; i--)
                    {
                        float r = Vec3.Distance(copy[i + 1], copy[i]);
                        float lambda = distances[i] / r;
                        copy[i] = (1 - lambda) * copy[i + 1] + lambda * copy[i];
                    }
                    //backward Reaching
                    copy[0] = StartPos;
                    for(int i = 0; i < copy.Length - 1; i++)
                    {
                        float r = Vec3.Distance(copy[i + 1], copy[i]);
                        float lambda = distances[i] / r;
                        copy[i + 1] = (1 - lambda) * copy[i] + (lambda * copy[i + 1]);
                    }

                    //comprovacion de distancia
                    targetEndEDistance = Vec3.Distance(target, copy[copy.Length - 1]);
                 }
            }
        }


    }

    public void actualize(){
        //actualizamos sus joints
        updateJoints();
        //actualizamos todos sus branches
        for(int i = 0; i < subBranches.Length; i++)
        {
            subBranches[i].actualize();
        }

    }

    public void updateJoints()
    {
        joints[0].position =(Vector3) copy[0];
        for(int i = 0; i < joints.Length - 1; i++)
        {
            Vec3 init = (Vec3)(joints[i + 1].position - joints[i].position);
            Vec3 now = copy[i + 1] - copy[i];
            if (init != now)
            {
                // Vec3 Axis = Vec3.crossProduct(init.normalized(), now.normalized());
                init.normalize();
                now.normalize();
                Vec3 Axis = Vec3.crossProduct(init, now);
                float cosa = Vec3.dotProduct(init, now);

                float sina = Axis.magnitude();

                float angle = Mathf.Atan2(sina, cosa);


                //if (Axis.magnitude() > 0) { 
                    Axis.normalize();
                    joints[i].rotation = (Quaternion)(new Quat(Axis, angle) * (Quat)joints[i].rotation);
                // }
                Debug.DrawLine(joints[i].position, joints[i + 1].position, Color.red);
                joints[i + 1].position = (Vector3)copy[i + 1];
            }

        }
    }



}
