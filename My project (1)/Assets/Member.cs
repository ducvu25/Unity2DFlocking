using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Member : MonoBehaviour
{
    public Vector3 positon;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig config;

    Vector3 wander_target;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<Level>();
        config = FindObjectOfType<MemberConfig>();

        positon = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration =  Combine();// Cohesion();//Wander();
        acceleration = Vector3.ClampMagnitude(acceleration, config.max_acceleration);
        velocity = velocity + acceleration*Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, config.max_velocity);
        positon = positon + velocity*Time.deltaTime;
        WrapAround(ref positon, -level.bounds, level.bounds);
        transform.position = positon;
    }
    protected Vector3 Wander()
    {
        float jitter = config.wander_jitter * Time.deltaTime;
        wander_target += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
        wander_target = wander_target.normalized;
        wander_target *= config.wander_radius;

        Vector3 target_in_local_space = wander_target + new Vector3(0, config.wander_distance, 0);
        Vector3 target_in_world_space = transform.TransformPoint(target_in_local_space);
        return target_in_world_space.normalized;
    }
    Vector3 Cohesion()
    {
        Vector3 cohesion_vector = new Vector3();
        int count_members = 0;
        var neighbors = level.GetNeighbors(this, config.conhesion_radius);
        if(neighbors.Count == 0)
        {
            return cohesion_vector;
        }
        foreach(var neighbor in neighbors)
        {
            if (IsInFOV(neighbor.positon))
            {
                cohesion_vector += neighbor.positon;
                count_members++;
            }
        }
        if(count_members == 0)
            return cohesion_vector;
        cohesion_vector /= count_members;
        cohesion_vector = cohesion_vector - this.positon;
        cohesion_vector = Vector3.Normalize(cohesion_vector);
        return cohesion_vector;
    }
    Vector3 Alignment()
    {
        Vector3 alignment_vector = new Vector3();
        var member = level.GetNeighbors(this, config.alignment_radius);
        if (member.Count == 0)
            return alignment_vector;
        foreach (var neighbor in member)
        {
            if (IsInFOV(neighbor.positon))
                alignment_vector += neighbor.velocity;
        }
        return alignment_vector.normalized;
    }
    Vector3 Separation()
    {
        Vector3 separate_vetor = new Vector3();
        var member = level.GetNeighbors(this, config.alignment_radius);
        if (member.Count == 0)
            return separate_vetor;
        foreach (var neighbor in member)
        {
            if (IsInFOV(neighbor.positon))
            {
                Vector3 moving_towards = this.positon - neighbor.positon;
                if(moving_towards.magnitude > 0)
                {
                    separate_vetor += moving_towards.normalized/ moving_towards.magnitude;
                }
            }
        }
        return separate_vetor.normalized;
    }
    Vector3 Avoidance()
    {
        Vector3 avoidance = new Vector3();
        var enemyList = level.GetEnemies(this, config.avoidane_radius);
        if (enemyList.Count == 0)
            return avoidance;
        foreach (var enemy in enemyList)
        {
            avoidance += RunAway(enemy.positon);
        }
        return avoidance.normalized;
    }
    Vector3 RunAway(Vector3 target)
    {
        Vector3 needed_velocity = (positon - target).normalized * config.max_velocity;
        return needed_velocity - velocity;
    }
    virtual protected Vector3 Combine()
    {
        Vector3 final_vec = new Vector3();
        final_vec += config.conhesion_priority * Cohesion() * level.regime[0] 
                   + config.wander_priority * Wander() * level.regime[1]
                   + config.alignment_priority*Alignment() * level.regime[2] 
                   + config.separition_priority*Separation() * level.regime[3]
                   + config.avoidane_priority*Avoidance() * level.regime[4];
        return final_vec;
    }

    void WrapAround(ref Vector3 vector, float min, float max)
    {
        vector.x = WrapAroundFloat(vector.x, min, max);
        vector.y = WrapAroundFloat(vector.y, min, max);
        vector.z = WrapAroundFloat(vector.z, min, max);
    }
    float WrapAroundFloat(float value, float min, float max)
    {
        value = value > max? min : (value < min ? max: value);
        return value;
    }
    float RandomBinomial()
    {
        return Random.Range(0, 1) - Random.Range(0, -1);
    }
    bool IsInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.velocity, vec - this.positon) <= config.max_FOV;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
