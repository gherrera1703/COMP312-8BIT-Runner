using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [Header("Jump forgiveness")]
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.15f;
    private Rigidbody2D rb;
    private float horizontalInput;
    private float lastGroundedTime = float.NegativeInfinity;
    private float lastJumpPressedTime = float.NegativeInfinity;
    private float ignoreGroundUntil;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[12];

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        if (Time.timeScale <= 0f) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0f)
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1f, 1f);
        if (Input.GetButtonDown("Jump")) lastJumpPressedTime = Time.time;
    }

    void FixedUpdate()
    {
        var filter = new ContactFilter2D();
        filter.useTriggers = false;
        int count = rb.Cast(Vector2.down, filter, groundHits, 0.06f);
        if (Time.time >= ignoreGroundUntil && rb.linearVelocity.y <= 0.1f)
        {
            for (int i = 0; i < count; i++)
            {
                if (groundHits[i].normal.y > 0.5f && groundHits[i].collider.CompareTag("Ground"))
                {
                    lastGroundedTime = Time.time;
                    break;
                }
            }
        }
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        bool bufferedJump = Time.time - lastJumpPressedTime <= jumpBufferTime;
        bool canJump = Time.time - lastGroundedTime <= coyoteTime;
        if (bufferedJump && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpPressedTime = float.NegativeInfinity;
            lastGroundedTime = float.NegativeInfinity;
            ignoreGroundUntil = Time.time + 0.1f;
        }
    }

    public void ResetAfterRespawn()
    {
        horizontalInput = 0f;
        lastJumpPressedTime = float.NegativeInfinity;
        lastGroundedTime = float.NegativeInfinity;
        ignoreGroundUntil = Time.time + 0.1f;
    }
}
