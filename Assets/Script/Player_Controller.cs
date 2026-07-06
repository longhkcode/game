using System;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim;
    

    [SerializeField] private float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        HandMovement();
    }

    void HandMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed);
        
        bool isMoving = (moveX != 0 || moveY != 0);
        anim.SetBool("isMoving", isMoving);
        
        if (isMoving)
        {
            if (moveY > 0)
            {
                anim.SetInteger("Direction", 1); // Up
            }
            else if (moveY < 0)
            {
                anim.SetInteger("Direction", 0); // Down
            }
            else if (moveX != 0)
            {
                anim.SetInteger("Direction", 2); // RightLeft

                if (moveX > 0)
                    sr.flipX = false; // Right
                else
                    sr.flipX = true;  // Left
            }
        }
    }
}