using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABRIK_MultiEnd : MonoBehaviour {
    public float threshHold;
    public int maxIterations;
    public Transform baseJoint;
    public Branch[] branches;
    
    

    void Start()
    {
        Vec3 prueba = new Vec3(1, 1, 1);
        Vec3 prueba2 = new Vec3(1, 1, 1);
       
        //Debug.Log(branches.Length);
        for (int i = 0; i < branches.Length; i++)
        {
            branches[i].init();
        }   
    }

	void Update () {
        //inverseReaching
        for (int i = 0; i < branches.Length; i++)
            branches[i].inverseReaching(threshHold);
        //normalAlgorithm
        for (int i = 0; i < branches.Length; i++)
            branches[i].normalReaching((Vec3)baseJoint.position,threshHold,maxIterations);

        //assign new positions and orientations
        for(int i=0; i < branches.Length; i++)
        {
            branches[i].actualize();
        }

	}

    void AddBranch()
    {

    }


}
