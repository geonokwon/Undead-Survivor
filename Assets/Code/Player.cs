using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable() {
        speed *= Charactor.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void FixedUpdate() {
        //1. 힘을준다
        // rigid.AddForce(inputVec);
        //2. 속도 제어
        // rigid.velocity = inputVec;

        //3. 위치 이동
        if (!GameManager.instance.isLive) {
            return;
        }
        Vector2 nextVec = inputVec * (speed * Time.fixedDeltaTime);
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate() {
        //magnitude : 벡터의 순수한 크기!
        if (!GameManager.instance.isLive) {
            return;
        }
        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) {
            sprite.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value) {
        inputVec = value.Get<Vector2>();
    }

    void OnCollisionStay2D() {
        if (!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime * 30f;

        if (GameManager.instance.health < 0) {
            for (int i = 2; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}