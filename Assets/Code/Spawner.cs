using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {
    public Transform[] spawnPoints;
    public SpawnData[] spawnData;
    
    private int level;
    private float timer;

    //=== test
    public Text text;
    public Text fpsText;
    public int enemyCount;
    private float deltaTime = 0.0f;


    void Awake() {
        spawnPoints = GetComponentsInChildren<Transform>();
    }


    void Update() {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
        if (timer > spawnData[level].spawnTime) {
            timer = 0f;
            Spawn();
        }

        //=== test
        //fps Test for Window
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        //fps 계산
        float fps = 1.0f / deltaTime;

        fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
    }

    void Spawn() {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        var sc = enemy.GetComponent<Enemy>();
        sc.Init(spawnData[level]);
        sc.spawner = this;
        

        //=== test
        if (enemyCount <= 100000000) {
            enemyCount++;
            text.text = string.Format("count:{0}", enemyCount);
        }
    }

    void decreaseEnemyCount() {
        enemyCount--;
        if (enemyCount <= 100000000) {
            text.text = string.Format("count:{0}", enemyCount);
        }
    }
}

[System.Serializable]
public class SpawnData {
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}