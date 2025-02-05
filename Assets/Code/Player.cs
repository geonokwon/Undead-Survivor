using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    
    void FixedUpdate()
    {
        //1. 힘을준다
        // rigid.AddForce(inputVec);
        //2. 속도 제어
        // rigid.velocity = inputVec;
        
        //3. 위치 이동
        Vector2 nextVec = inputVec * (speed * Time.fixedDeltaTime);
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        //magnitude : 벡터의 순수한 크기!
        animator.SetFloat("Speed", inputVec.magnitude);
        
        if (inputVec.x != 0 )
        {
            sprite.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    
}
