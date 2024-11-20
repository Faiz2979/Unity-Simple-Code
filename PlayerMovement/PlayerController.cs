using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody2D rb;
    public Animator anim;
    [Header("Player Movement")]
    public float xInput;
    public float yInput;
    public bool jump;
    public float dashSpeed;
    public float speed;
    bool isMoving=false;

    [Header("Player Facing")]
    private int facingDir = 1;
    private bool facingRight = true;
    public float jumpForce;
    public Transform groundCheck;
    public float rayCastLong;
    public LayerMask groundLayer;
    public bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        flipPlayer();
    }

    void HandleMovement(){
        CheckGroundStatus();
        GetInput();
        MovePlayer();
        UpdateAnimations();
        HandleJumpAndFall();
    }

    void CheckGroundStatus() {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, rayCastLong, groundLayer);
    }

    void GetInput() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    void MovePlayer() {
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
    }

    void UpdateAnimations() {
        isMoving = xInput != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }


    void HandleJumpAndFall() {
        if ((jump && isGrounded) || (yInput > 0 && isGrounded)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (yInput < 0) {
            if (!isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + yInput * (speed/2));
            } 
        }
    }

void flipPlayer()
{
    if (xInput > 0 && !facingRight)  // Jika bergerak ke kanan dan menghadap ke kiri, lakukan flip
    {
            flip();
    }
    else if (xInput < 0 && facingRight)  // Jika bergerak ke kiri dan menghadap ke kanan, lakukan flip
    {
        flip();
    }
}

    // Fungsi untuk melakukan flip player
    void flip()
    {
        facingDir *= -1;  // Ubah arah menghadap
        facingRight = !facingRight;  // Toggle arah
        transform.Rotate(0f, 180f, 0f);  // Putar player 180 derajat di sumbu Y
    }

        private void OnDrawGizmos()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * rayCastLong);
            }
        }
}
