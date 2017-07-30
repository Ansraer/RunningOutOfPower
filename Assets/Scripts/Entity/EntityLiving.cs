using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLiving : Entity
{


    public float defaultMovementSpeed = 4;


    //account for effects here
    public virtual float GetMovementSpeed()
    {
        return this.defaultMovementSpeed;
    }



}


