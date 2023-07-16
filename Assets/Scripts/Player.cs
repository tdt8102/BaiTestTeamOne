using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private float horizontalMove;
    private bool moveRight;
    private bool moveLeft;
    private Rigidbody2D rb;
    public float jumpSpeed = 5;
    bool isGrounded;
    bool canDoubleJump;
    public float delayBeforeDoubleJump;
    

    [SerializeField] private AudioSource jumpSoundEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveLeft = false;
        moveRight = false;
    }

    void Update()
    {
        Movement();
    }

    public void pointDownLeft()
    {
        moveLeft = true;
    }

    public void pointUpLeft()
    {
        moveLeft = false;
    }

    public void pointDownRight()
    {
        moveRight = true;
    }

    public void pointUpRight()
    {
        moveRight = false;
    }

    void Movement()
    {
        if(moveLeft)
        {
            horizontalMove = -speed;
        }
        else if(moveRight)
        {
            horizontalMove = speed;
        }
        else
        {
            horizontalMove = 0;
        }
    }

     void OnCollisionEnter2D(Collision2D other)
    {
            if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
        }
        else if (other.gameObject.CompareTag("Box"))
        {
            Vector2 contactNormal = other.GetContact(0).normal;
            float dotProduct = Vector2.Dot(contactNormal, Vector2.right);

            // Kiểm tra xem Player di chuyển vào hướng tường
            if (dotProduct > 0.5f && moveRight)
            {
                Rigidbody2D boxRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

                // Áp dụng lực đẩy cho hộp gỗ
                if (boxRigidbody != null)
                {
                    float pushForce = 10f; // Điều chỉnh độ mạnh của lực đẩy
                    Vector2 pushDirection = Vector2.right; // Xác định hướng đẩy

                    boxRigidbody.AddForce(pushForce * pushDirection, ForceMode2D.Impulse);
                }
            }
            else if (dotProduct < -0.5f && moveLeft)
            {
                Rigidbody2D boxRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

                // Áp dụng lực đẩy cho hộp gỗ
                if (boxRigidbody != null)
                {
                    float pushForce = 5f; // Điều chỉnh độ mạnh của lực đẩy
                    Vector2 pushDirection = Vector2.left; // Xác định hướng đẩy

                    boxRigidbody.AddForce(pushForce * pushDirection, ForceMode2D.Impulse);
                }
            }
        }
    }
    public void jumpButton()
    {
        if(isGrounded)
        {
            jumpSoundEffect.Play(); 
            isGrounded = false;
            rb.velocity = Vector2.up * jumpSpeed;
            Invoke("EnableDoubleJump", delayBeforeDoubleJump);
        }
        if (canDoubleJump)
        {
            jumpSoundEffect.Play(); 
            rb.velocity = Vector2.up * jumpSpeed;
            canDoubleJump = false;
        }
    }
 
    void EnableDoubleJump()
        {
            canDoubleJump = true;
        }
    private void FixedUpdate()
{
    rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
}

    
}
