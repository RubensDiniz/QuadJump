using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;

    public LayerMask groundLayer;
    public Transform leftFoot;
    public Transform rightFoot;
    private int shouldCheckGrounded;
    private float radius = 0.01f;
    public bool isGrounded => Physics2D.OverlapCircle(leftFoot.position, radius, groundLayer) ||
                              Physics2D.OverlapCircle(rightFoot.position, radius, groundLayer);

    public float walkingSpeed;
    float hinput;

    public float jumpingForce;
    int jumpCount;

    public Coroutine DashCoroutine;
    public float dashTime;
    public float dashForce;

    private float originalGravity;

    [Serializable]
    public enum PlayerState {
        Normal,
        Dashing,
        FallingSlowly,
        Paused
    }

    [SerializeField] private PlayerState _state;

    public PlayerState State {
        get => _state;

        set {

            Debug.Log(value);
            switch (value){
                case PlayerState.Normal:
                    rb.gravityScale = originalGravity;
                    break;
                case PlayerState.Dashing:
                    rb.gravityScale = 0;
                    break;
                case PlayerState.FallingSlowly:
                    break;
                case PlayerState.Paused:
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        jumpCount = 0;
        shouldCheckGrounded = 0;

        originalGravity = rb.gravityScale;
        State = PlayerState.Normal;
    }

    // Update is called once per frame
    void Update() {
        switch (State) {
            case PlayerState.Normal:
                MoveLogic();
                JumpLogic();
                break;
            case PlayerState.Dashing:
                JumpLogic();
                break;
            case PlayerState.FallingSlowly:
                break;
            case PlayerState.Paused:
                break;
        }

       // MoveLogic();
       // JumpLogic();
    }

    void FixedUpdate() {
        GroundedLogic();

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (DashCoroutine != null) {
            StopCoroutine(DashCoroutine);
            EndDash();
        }
    }

    private void MoveLogic() {
        hinput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(hinput * walkingSpeed, rb.velocity.y);
        //print(rb.velocity.x);
    }

    private void JumpLogic() {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 5) {
            shouldCheckGrounded = 3;

            switch (jumpCount) {
                case 0:
                case 1:
                    Jump();
                    break;
                case 2:
                    Dash();
                    break;
                case 3:
                    Shoot();
                    break;
            }

            jumpCount++;

            //Debug.Log(jumpCount);
        }
    }
    
    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
    }

    private void Dash() {
        DashCoroutine = StartCoroutine(DashLogic());
    }

    private IEnumerator DashLogic() {
        rb.velocity = new Vector2(dashForce, 0);
        State = PlayerState.Dashing;
        yield return new WaitForSeconds(dashTime);
        EndDash();
    }

    private void EndDash() {
        DashCoroutine = null;
        rb.gravityScale = originalGravity;
        //rb.velocity = new Vector2(0, 0);
        State = PlayerState.Normal;
    }

    private void Shoot() {

    }

    private void GroundedLogic() {
        if (shouldCheckGrounded <= 0 && isGrounded) {
            jumpCount = 0;
        } else if (shouldCheckGrounded > -5) {
            shouldCheckGrounded--;
        }
    }
}
