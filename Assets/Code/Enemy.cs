using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;
    
    Rigidbody2D rigid;
    Collider2D col;
    SpriteRenderer sprite;
    Animator animator;
    WaitForFixedUpdate wait;
    
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {   
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        sprite.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        

        if (health > 0) {
            // .. Live, Hit Action
            animator.SetTrigger("Hit");
        }
        else {
            // .. Die
            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    //생명 주기와 비동기처럼 실행되는 함수
    IEnumerator KnockBack()
    {
        // yield return null; // 1프레임 쉬기 
        // yield return new WaitForSeconds(2f); // 2초 쉬기
        
        //하나의 물리 프레임을 딜레이
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dir = transform.position - playerPos;
        rigid.AddForce(dir.normalized * 3, ForceMode2D.Impulse);
    }
    
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
