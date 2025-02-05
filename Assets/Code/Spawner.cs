using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    
    public Transform[] spawnPoints;
    public SpawnData[] spawnData;

    private int level;
    private float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }
    
    
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
        
        if (timer > spawnData[level].spawnTime) {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    { 
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
