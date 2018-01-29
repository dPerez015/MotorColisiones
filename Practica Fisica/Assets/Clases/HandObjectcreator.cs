using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectcreator : MonoBehaviour {

    public SceneManager manager;
    public GameObject[] targets;
    public GameObject[] openTargets;
    public GameObject[] closeTargets;

    GameObject heldObject;
    public GameObject[] posibleObjects;
    public GameObject generationPosition;

    float speed;
    float scalar;
    bool opened;
    bool isMoving;
    // Use this for initialization
    void Start () {
        isMoving = true;
        opened = true;
        speed = 0.5f;
        scalar = 1;
        for (int i=0; i < targets.Length; i++)
        {
            targets[i].transform.position = openTargets[i].transform.position;
        }
	}
	
    void lerpTarget(GameObject target, int pos)
    {
        //target.transform.position = ((1 - scalar)* closeTargets[pos].transform.position+ openTargets[pos].transform.position) ;
        target.transform.position = closeTargets[pos].transform.position + (openTargets[pos].transform.position - closeTargets[pos].transform.position) * scalar;
    }

    void generateObject()
    {
        int rand = (int)Mathf.Floor(Random.value*2);
        heldObject = Instantiate(posibleObjects[rand],generationPosition.transform.position,generationPosition.transform.rotation, generationPosition.transform);
        heldObject.GetComponent<PhysicalObject>().flecha = manager.flecha;
    }

    void releaseObject()
    {
        heldObject.GetComponent<PhysicalObject>().Initialisation();
        manager.addObject(heldObject.GetComponent<PhysicalObject>());
    }

	// Update is called once per frame
	void Update () {
        if (isMoving) {
            if (!opened)
            {
                scalar = Mathf.Clamp(scalar + speed * Time.deltaTime, 0, 1);
                if (scalar == 1)
                {
                    opened = true;
                    releaseObject();
                }
            }
            else {

                scalar = Mathf.Clamp(scalar - speed * Time.deltaTime, 0, 1);
                if (scalar == 0)
                {
                    isMoving = false;
                    opened = false;
                    generateObject();
                }
            }

            //movemos los targets
            for(int i=0; i < targets.Length; i++)
            {
                lerpTarget(targets[i], i);
            }
        }
        else
        {
            if (Input.GetAxisRaw("Fire1")==1)
            {
                isMoving = true;
                
            }
        }
	}
}
