using UnityEngine;
using System.Collections;

public class Mat3 {
    public float[,] matrix = new float[3, 3];

    public Mat3(float[,] values)
    {
        /*if(values.Length != 9)
        {
            Debug.Log("Invalid input values for the Mat3 initialisation");
            return;
        }*/
        matrix = values;
    }
     
}
