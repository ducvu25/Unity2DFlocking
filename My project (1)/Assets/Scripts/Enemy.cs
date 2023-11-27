using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Member
{
    protected override Vector3 Combine()
    {
        return config.wander_priority*Wander();
    }
    // Start is called before the first frame update
   
}
