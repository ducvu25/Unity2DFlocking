using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
    public Transform memberPrefab;
    public Transform enemyPrefab;
    public int numberOfMembers;
    public int numberOfEnemies;
    public List<Member> members;
    public List<Enemy> enemies;
    public float bounds;
    public float spawnRadius;
    public int[] regime;

    public TMP_InputField inputNumberMember;
    public TMP_InputField inputNumberEnemy;
    public TextMeshProUGUI txtNumberMember;
    public TextMeshProUGUI txtNumberEnemy;
    public Slider fps;

    // Start is called before the first frame update
    void Start()
    {
        regime = new int[5];
        for (int i = 0; i < regime.Length; i++)
            regime[i] = 1;

        SetUp();
    }
    void SetUp()
    {
        txtNumberEnemy.text = "Enemies: " + numberOfEnemies.ToString();
        txtNumberMember.text = "Member: " + numberOfMembers.ToString();
        // Tạo mới enemy và member
        Spawn(memberPrefab, numberOfMembers);
        Spawn(enemyPrefab, numberOfEnemies);
        members = new List<Member>(FindObjectsOfType<Member>());
        // Cập nhật danh sách enemy và member
        members = new List<Member>(FindMembers());
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    Member[] FindMembers()
    {
        var allMembers = FindObjectsOfType<Member>();
        var filteredMembers = new List<Member>();

        foreach (var member in allMembers)
        {
            if (!(member is Enemy))
            {
                filteredMembers.Add(member);
            }
        }

        return filteredMembers.ToArray();
    }

    void Spawn(Transform prefab, int number)
    {
        if (number > 0)
        {
            Instantiate(prefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), Quaternion.identity);
            Spawn(prefab, number - 1);
        }
    }

    public List<Member> GetNeighbors(Member member, float radius)
    {
        List<Member> neigh = new List<Member>();

        foreach (var i in members)
            if (i != member && Vector3.Distance(member.transform.position, i.transform.position) <= radius)
                neigh.Add(i);
        return neigh;
    }
    public List<Enemy> GetEnemies(Member member, float radius) {
        List<Enemy> neigh = new List<Enemy>();
        foreach (var i in enemies)
        {
            if(Vector3.Distance(member.positon, i.positon) <= radius)
            {
                neigh.Add((Enemy)i);
            }
        }
        return neigh;
    }
    public void SetRegime(int i)
    {
        regime[i] = regime[i] == 0 ? 1 : 0;
    }
    public void UpdateSpeed()
    {
        Time.timeScale = fps.value;
    }
    public void ResetGame()
    {
        numberOfEnemies = int.Parse(inputNumberEnemy.text != "" ? inputNumberEnemy.text : "0");
        Debug.Log(numberOfEnemies);
        numberOfMembers = int.Parse(inputNumberMember.text != "" ? inputNumberMember.text : "0");

        inputNumberEnemy.text = "";
        inputNumberMember.text = "";
        // Hủy bỏ các enemy và member hiện tại
        foreach (var member in members)
        {
            Destroy(member.gameObject);
        }
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        members.Clear();
        enemies.Clear();
        Invoke("SetUp", 2f);
       // SetUp();
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
