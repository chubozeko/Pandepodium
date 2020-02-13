using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reddy : MonoBehaviour
{
    public float speed = 5;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    public bool facingRight = true;

    public float jumpSpeed = 5f;

    public bool isJumping = false;

    private float rayCastLength = 0.005f;

    private float width;
    private float height;

    private float jumpButtonPressTime;

    private float maxJumpTime = 0.2f;

    // Wall Jumps
    public float wallJumpY = 10f;

    void Start()
    {
        
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.1f;
    }

    void FlipReddy()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsOnGround()
    {
        bool groundCheck1 = Physics2D.Raycast(new Vector2(
            transform.position.x,
            transform.position.y - height),
            -Vector2.up,
            rayCastLength);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(
            transform.position.x + (width - 0.2f),
            transform.position.y - height),
            -Vector2.up,
            rayCastLength);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(
            transform.position.x - (width - 0.2f),
            transform.position.y - height),
            -Vector2.up,
            rayCastLength);

        if(groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnBecameInvisible()
    {
        //Destroy(gameObject);
    }

    public bool IsWallOnLeft()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x - width,
            transform.position.y),
            -Vector2.right,
            rayCastLength);
    }

    public bool IsWallOnRight()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + width,
            transform.position.y),
            Vector2.right,
            rayCastLength);
    }

    public bool IsWallOnLeftOrRight()
    {
        if(IsWallOnLeft() || IsWallOnRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWallDirection()
    {
        if (IsWallOnLeft())
        {
            return -1;
        }
        else if (IsWallOnRight())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void FixedUpdate()
    {
        float horzMove = Input.GetAxisRaw("Horizontal");
        Vector2 vect = rb.velocity;
        rb.velocity = new Vector2(horzMove * speed, vect.y);

        if(IsWallOnLeftOrRight() && !IsOnGround() && horzMove == 1)
        {
            rb.velocity = new Vector2(-GetWallDirection() * speed * -.75f, wallJumpY);
        }

        animator.SetFloat("Speed", Mathf.Abs(horzMove));

        if(horzMove > 0 && !facingRight)
        {
            FlipReddy();
        } 
        else if (horzMove < 0 && facingRight)
        {
            FlipReddy();
        }

        float vertMove = Input.GetAxisRaw("Vertical");
        
        if(IsOnGround() && isJumping == false)
        {
            if(vertMove > 0f)
            {
                isJumping = true;
            }
        }

        if(jumpButtonPressTime > maxJumpTime)
        {
            vertMove = 0f;
        }

        if(isJumping && jumpButtonPressTime < maxJumpTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if(vertMove >= 1f)
        {
            jumpButtonPressTime += Time.deltaTime;
        }
        else
        {
            isJumping = false;
            jumpButtonPressTime = 0f;
        }
    }
}
