

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // Controls how fast the enemy patrols
    public float moveSpeed = 1.5f;

    // Controls how far the enemy patrols
    public float patrolDistance = 1.5f;

    // Controls when the enemy detects the player
    public float detectionRange = 3f;

    // Controls how fast the enemy chases the player
    public float chaseSpeed = 2f;

    // Controls how close the enemy gets before stopping
    public float stoppingDistance = 0.8f;

    // How long the enemy stops moving after being hit
    public float knockbackDuration = 0.3f;

    // Stores the enemy's Rigidbody2D
    private Rigidbody2D rb;

    // Stores the player's position
    private Transform player;

    // Remembers the enemy's starting horizontal position
    private float startingX;

    // Tracks which direction the enemy moves
    private int direction = 1;

    // Tracks when the knockback ends
    private float knockbackEndTime = 0f;

    // Runs when the game starts
    void Start()
    {
        // Find the enemy's Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Save the starting position
        startingX = transform.position.x;

        // Find the player
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Runs at fixed intervals for physics movement
    void FixedUpdate()
    {
        // Allow physics to control movement during knockback
        if (Time.time < knockbackEndTime)
        {
            return;
        }

        // Check whether the player is nearby
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(
                rb.position,
                player.position
            );

            // Chase the player if within detection range
            if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
                return;
            }
        }

        // Patrol when the player is not nearby
        Patrol();
    }

    // Makes the enemy patrol left and right
    void Patrol()
    {
        // Turn around at the right patrol boundary
        if (rb.position.x >= startingX + patrolDistance)
        {
            direction = -1;
        }

        // Turn around at the left patrol boundary
        else if (rb.position.x <= startingX - patrolDistance)
        {
            direction = 1;
        }

        // Move horizontally without removing gravity
        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );
    }

    // Makes the enemy chase the player
    void ChasePlayer()
    {
        // Calculate horizontal distance to the player
        float horizontalDistance =
            player.position.x - rb.position.x;

        // Stop moving when close enough to attack
        if (Mathf.Abs(horizontalDistance) <= stoppingDistance)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            return;
        }

        // Choose which direction to chase
        direction = horizontalDistance > 0 ? 1 : -1;

        // Move toward the player while preserving gravity
        rb.linearVelocity = new Vector2(
            direction * chaseSpeed,
            rb.linearVelocity.y
        );
    }

    // Called by EnemyHealth when the enemy gets hit
    public void StartKnockback()
    {
        // Temporarily disable patrol and chase movement
        knockbackEndTime = Time.time + knockbackDuration;
    }
}
