using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolution  {


    public CollisionResolution()
    {

    }

    void adjustVelocity(CollisionData collision)
    {
        //encontrar el set de coordenadas relativo al contacto
        collision.calculateContactBasis();

        //calcular el cambio de velocidad por unidad de impulso 
      //  Vec3 torquePerUnitImpulse = r

        //calcular el cambio de velocidad necesario

        //caluclar impulso necesario para ese cambio

        //aplicar impulso a cada objeto
    }

    public void solveCollisions(List<CollisionData> collisions)
    {
        foreach(CollisionData collision in collisions)
        {
          //  solve
        }
    }
    
}
