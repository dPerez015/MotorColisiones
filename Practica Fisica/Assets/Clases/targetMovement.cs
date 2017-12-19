using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetMovement : MonoBehaviour {

    public float size;
    public float speed;
    public Vector3 direction;
    public Vec3 Direction;


    private Vec3 startPos;
    public float startAngle;
	void Start () {
        startPos = (Vec3)transform.position;
        //startAngle *= Mathf.Deg2Rad;
        Direction = (Vec3)direction.normalized;
 
	}
	
	// Update is called once per frame
	void Update () {
        startAngle = (startAngle + (speed * Time.deltaTime));
        Vec3 move = Direction * Mathf.Sin(startAngle* Mathf.Deg2Rad)*size;
        transform.position = (Vector3)(startPos + move);
	}
}
