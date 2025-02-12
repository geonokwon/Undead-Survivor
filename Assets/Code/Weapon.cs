using UnityEngine;

public class Weapon : MonoBehaviour {
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private float timer;
    private Player player;

    private void Awake() {
        player = GameManager.instance.player;
    }

    private void Update() {
        if (!GameManager.instance.isLive) return;
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }

                break;
        }

        //.. Test Code ..
        if (Input.GetButtonDown("Jump")) {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count) {
        this.damage = damage * Charactor.Damage;
        this.count += count;
        if (id == 0) Batch();
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }


    public void Init(ItemData data) {
        //Basic Set
        name = "Weapon " + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        id = data.itemID;
        damage = data.baseDamage * Charactor.Damage;
        count = data.baseCount + Charactor.Count;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++) {
            if (data.projectile == GameManager.instance.pool.prefabs[i]) {
                prefabId = i;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = 150 * Charactor.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f * Charactor.WeaponRate;
                break;
        }
        
        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch() {
        for (int index = 0; index < count; index++) {
            Transform bullet;
            if (index < transform.childCount) {
                bullet = transform.GetChild(index);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire() {
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
        
        AudioManger.instance.PlaySfx(AudioManger.Sfx.Range);
    }
}