using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberConfig : MonoBehaviour
{
    public float max_FOV = 180;
    public float max_acceleration;
    public float max_velocity;

    // Wander variables
    public float wander_jitter;
    public float wander_radius;
    public float wander_distance;
    public float wander_priority;

    // Cohesion Variables
    public float conhesion_radius;
    public float conhesion_priority;

    // Alignment Variables
    public float alignment_radius;
    public float alignment_priority;

    // Separition Variables
    public float separition_radius;
    public float separition_priority;

    // Avoidance Variable
    public float avoidane_radius;
    public float avoidane_priority;
    
}
