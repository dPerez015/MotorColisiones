using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour {

    Vec3 end;
    Vec3 origin;
    Color color;
    Quat rotation;
    Vec3 direction;
    PhysicalObject obj;
    public void init(Vec3 orig, Vec3 fin, Color col,PhysicalObject ob)
    {
        obj = ob;
        origin = orig;
        color = col;
        direction = fin - origin;
        direction /= 12;
        if (direction.magnitude() < 0.5)
            direction = direction.normalized() / 2;
        end = origin + direction;

        float angle = Mathf.Acos(Vec3.dotProduct(new Vec3(0, 0, 1), direction.normalized()))*Mathf.Rad2Deg;
        Vec3 axis = Vec3.crossProduct(direction.normalized(),new Vec3(0, 0, 1));

        rotation = new Quat(axis.normalized(),angle);
        transform.rotation = (Quaternion)rotation;
        transform.position = (Vector3)(end - (direction.normalized() * 0.05f));

        GetComponent<Renderer>().material.SetColor("_Color", col);
        GetComponent<LineRenderer>().material.SetColor("_Color", col);
    }
	
	public void customUpdate (Vec3 position, Vec3 force) {
        origin = position;
        direction = force;
        direction /= 12;
        if (direction.magnitude() < 0.5)
            direction = direction.normalized() / 2;
        end = origin + direction;

        float angle = Mathf.Acos(Vec3.dotProduct(new Vec3(0, 0, 1), direction.normalized())) * Mathf.Rad2Deg;
        Vec3 axis = Vec3.crossProduct( new Vec3(0, 0, 1), direction.normalized());

        rotation = new Quat(axis.normalized(), angle);
        transform.rotation = (Quaternion)rotation;
        transform.position = (Vector3)(end - (direction.normalized() * 0.05f));
        //Debug.DrawLine((Vector3)origin, (Vector3)end, color);
      
        GetComponent<LineRenderer>().SetPosition(0,(Vector3)origin);
        GetComponent<LineRenderer>().SetPosition(1, (Vector3)end);
	}
}
