using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;

    public LayerMask groundLayer;
    public float radius = 0.1f;
    public Transform leftFoot;
    public Transform rightFoot;
    public bool isGrounded => Physics2D.OverlapCircle(leftFoot.position, radius, groundLayer) ||
                              Physics2D.OverlapCircle(rightFoot.position, radius, groundLayer);

    public float walkingSpeed = 8f;
    float hinput;

    public float jumpingForce = 20f;
    int jumpCount;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        jumpCount = 0;
    }

    void FixedUpdate() {
        CheckGrounded();
    }

    // Update is called once per frame
    void Update() {
        Move();
        Jump();
    }

    private void Move() {
        hinput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(hinput * walkingSpeed, rb.velocity.y);
        
        //print(rb.velocity.x);
    }

    private void CheckGrounded() {
        //isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.4f, transform.position.y - 0.49f),
                                           //new Vector2(transform.position.x + 0.4f, transform.position.y - 0.49f), groundLayer);

        if (isGrounded) {
            jumpCount = 0;
        }
    }

    private void Jump() {
        
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 5) {
            switch (jumpCount) {
                case 0:
                case 1:
                    rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
                    jumpCount++;
                    break;
                case 2:
                    jumpCount++;
                    break;
                case 3:
                    jumpCount++;
                    break;
            }

            Debug.Log(jumpCount);
        }
    }
}
