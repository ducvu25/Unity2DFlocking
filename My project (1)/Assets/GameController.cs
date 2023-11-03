using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform memberPrefab;
    public Transform enemyPrefab;
    public int numberOfMembers;
    public int numberOfEnemies;
    public List<Member> members;
    public List<Enemy> enemies;
    public float bounds;
    public float spawnRadius;

    // Start is called before the first frame update
    void Start()
    {
        members = new List<Member>();
        enemies = new List<Enemy>();

        Spawn(memberPrefab, numberOfMembers);
        Spawn(enemyPrefab, numberOfEnemies);

        members.AddRange(FindObjectsOfType<Member>());
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }
    void Spawn(Transform prefab, int number)
    {
        Instantiate(prefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity);
        if(number > 0)
            Spawn(prefab, number - 1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Member> GetNeighbors(Member member, float radius)
    {
        List<Member> neigh = new List<Member>();

        foreach (var i in members)
            if (i != member && Vector3.Distance(member.transform.position, i.transform.position) <= radius)
                neigh.Add(i);
        return neigh;
    }
}
