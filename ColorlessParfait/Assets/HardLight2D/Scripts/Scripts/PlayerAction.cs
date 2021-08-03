using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : MonoBehaviour
{
    float speed = 5f;
    float runningSpeed = 10f;
    public float jumpPower = 1f;
    
    public Rigidbody2D rigid;
    public SpriteRenderer render;
    public Animator anim;
    public GameManager gameManager;
    
    private bool isJumping = false;
    private bool isOnGround = false;
    private bool isOnCafeDoor = false;
    void Start()
    {
        
    }

    public void Update()
    {
        if( Input.GetAxisRaw("Horizontal") != 0 )
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed = runningSpeed;
            anim.SetFloat("runSpeed", 1.5f);
        }
        else
        {
            speed = 5;
            anim.SetFloat("runSpeed", 1.0f);
        }

        if (isOnCafeDoor && Input.GetKeyDown(KeyCode.W))//카페로 들어가기
        {
            SceneManager.LoadScene("Cafe");
        }

        Attack();
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
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //점프(위쪽으로 힘 주기)
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

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !isOnGround)
        {
            Debug.Log("낙하공격!!!");
        }
        else if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && anim.GetBool("isRunning"))
        {
            Debug.Log("달리면서 공격!!!");
        }
        else if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Debug.Log("공격!!!!!");
            anim.SetTrigger("attack");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")//캐릭터가 땅에 닿아있을 때
        {
            anim.SetBool("isOnGround", true);
            isOnGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "CafeDoor"){//캐릭터가 카페 문 앞에 서 있을 때
            isOnCafeDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "CafeDoor"){//캐릭터가 카페 문 앞에서 떨어졌을 때
            isOnCafeDoor = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")//캐릭터가 땅에 막 닿았을 때
            anim.SetTrigger("grounding");
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")//캐릭터가 땅에서 떨어졌을 때
        {
            anim.SetBool("isOnGround", false);
            Invoke("setOnGroundFalse", 0.1f);
        }
    }

    public void setOnGroundFalse()
    {
        isOnGround = false;
    }
}