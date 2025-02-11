using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [Header("# Game Control")] public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")] public int health;
    public int maxHealth;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# GameObject")] public PoolManager pool;
    public Player player;

    void Awake() {
        maxHealth = 100;
        instance = this;
    }

    void Start() {
        health = maxHealth;
    }

    void Update() {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime) {
            gameTime = maxGameTime;
        }
    }

    public void GetExp() {
        exp++;
        if (exp == nextExp[level]) {
            level++;
            exp = 0;
        }
    }
}