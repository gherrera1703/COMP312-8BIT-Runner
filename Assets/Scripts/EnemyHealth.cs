
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Maximum health the enemy can have
    public int maxHealth = 100;

    // Current health remaining
    public int currentHealth;

    // Controls how far the enemy gets pushed
    public float knockbackForce = 5f;

    // Controls how high the enemy jumps when hit
    public float knockbackUpwardForce = 2f;

    // Stores the enemy's Rigidbody2D
    private Rigidbody2D rb;

    // Stores the enemy's patrol and chase script
    private EnemyPatrol enemyPatrol;

    // Runs when the game starts
    void Start()
    {
        // Give the enemy full health
        currentHealth = maxHealth;

        // Find the enemy's physics component
        rb = GetComponent<Rigidbody2D>();

        // Find the enemy's patrol script
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    // Runs whenever the enemy takes damage
    public void TakeDamage(int damage)
    {
        // Reduce the enemy's health
        currentHealth = Mathf.Max(0, currentHealth - damage);

        // Display remaining health
        Debug.Log("Enemy Health: " + currentHealth);

        // Destroy the enemy if its health reaches zero
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
            return;
        }

        // Find the player to determine knockback direction
        GameObject player = GameObject.Find("Player");

        // Only apply knockback when required components exist
        if (player != null && rb != null)
        {
            // Pause the enemy's patrol and chase movement
            if (enemyPatrol != null)
            {
                enemyPatrol.StartKnockback();
            }

            // Determine which direction is away from the player
            float direction =
                transform.position.x >= player.transform.position.x
                ? 1f : -1f;

            // Clear existing velocity before knockback
            rb.linearVelocity = Vector2.zero;

            // Push the enemy backward and upward
            rb.AddForce(
                new Vector2(
                    direction * knockbackForce,
                    knockbackUpwardForce
                ),
                ForceMode2D.Impulse
            );
        }
    }
}
