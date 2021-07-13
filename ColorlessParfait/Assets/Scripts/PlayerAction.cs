using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed = 10f;
    public float jumpPower = 1f;
    
    public Rigidbody2D rigid;
    public SpriteRenderer render;
    public Animator anim;
    
    private bool isJumping = false;
    private bool isOnGround = false;
    void Start()
    {
        
    }

    public void Update()
    {
        if( Input.GetAxisRaw("Horizontal") != 0 )
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Sit();
    }

    public void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x, 0, 0) * speed * Time.deltaTime;

        if (x < 0)
            render.flipX = false;
        else if (x > 0)
            render.flipX = true;

        transform.position += move;
    }

    public void Jump()
    {
        if (Input.GetButton("Jump") && !isJumping && isOnGround ) //점프 키가 눌렸고 캐릭터가 바닥에 닿아 있을 때
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //위쪽으로 힘을 준다.
            isJumping = true;
            anim.SetTrigger("jump");
            anim.SetBool("isJumping", true);
        }
        else {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }

    public void Sit()
    {
        if (Input.GetKey(KeyCode.S) && !isJumping && isOnGround)
        {
            anim.SetBool("isSitting", true);
        }
        else
        {
            anim.SetBool("isSitting", false);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")//캐릭터가 땅에 닿아있을 때
            isOnGround = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")//캐릭터가 땅에서 떨어졌을 때
        {
            Invoke("setOnGroundFalse", 0.1f);
        }
    }

    public void setOnGroundFalse()
    {
        isOnGround = false;
    }
}