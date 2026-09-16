using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 11f;
    public int maxJumps = 2;              // 支持几段跳（2 => 双跳）
    public float coyoteTime = 0.12f;      // 离地短时间仍可跳（宽容时间）
    public float jumpBufferTime = 0.12f;  // 按键缓冲（在落地前按下还能跳）

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    int jumpsLeft;
    float coyoteTimer;
    float jumpBufferTimer;
    bool wantToJump;

    // 移动平台支持：若站在平台上，记录平台当帧位移并把它加给玩家
    Vector2 platformDeltaThisFrame;
    MovingPlatform standOnPlatform; // optional reference

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        // input
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 vel = rb.velocity;
        vel.x = h * moveSpeed;
        rb.velocity = new Vector2(vel.x, rb.velocity.y); // horizontal set, vertical by physics

        // jump input & buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
            wantToJump = true;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // ground check
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpsLeft = maxJumps;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        // add platform delta if standing on a moving platform
        if (standOnPlatform != null)
        {
            Vector2 delta = standOnPlatform.DeltaMove;
            // apply platform movement to rigidbody position (kinematic approach)
            rb.position += delta;
        }

        // jump resolution (buffer + coyote + jumps left)
        if (wantToJump && jumpBufferTimer > 0f)
        {
            bool canGroundJump = coyoteTimer > 0f;
            if (canGroundJump)
            {
                DoJump();
            }
            else if (jumpsLeft > 1) // in-air additional jumps (double jump)
            {
                DoJump();
            }
            wantToJump = false;
            jumpBufferTimer = 0f;
        }
    }

    void DoJump()
    {
        // set vertical velocity directly for consistent jump height
        Vector2 v = rb.velocity;
        v.y = jumpForce;
        rb.velocity = v;

        jumpsLeft = Mathf.Max(0, jumpsLeft - 1);
        coyoteTimer = 0f;
    }

    // Callbacks for platform detection (Platform should call these)
    public void SetStandingPlatform(MovingPlatform platform)
    {
        standOnPlatform = platform;
    }
    public void ClearStandingPlatform(MovingPlatform platform)
    {
        if (standOnPlatform == platform) standOnPlatform = null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
