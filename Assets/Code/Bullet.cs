using Unity.Cinemachine;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Dead();
    }

    void Dead()
    {
        Transform target = GameManager.instance.player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if(dir > 20f)
            this.gameObject.SetActive(false);
    }
    
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1 )
        {
            rigid.linearVelocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1) return;
        per--;
        if (per == -1) {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }
    
    
    
    
}
