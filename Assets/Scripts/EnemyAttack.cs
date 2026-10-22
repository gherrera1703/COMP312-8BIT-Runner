
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // How close the player must be for an attack
    public float attackRange = 1.5f;

    // How much damage each attack deals
    public int attackDamage = 10;

    // How long the enemy waits between attacks
    public float attackCooldown = 1.5f;

    // Stores the player's position
    private Transform player;

    // Stores the player's health system
    private PlayerHealth playerHealth;

    // Tracks when the enemy can attack again
    private float nextAttackTime = 0f;

    // Runs once when the game starts
    void Start()
    {
        // Find the Player object in the scene
        GameObject playerObject = GameObject.Find("Player");

        // Save the player's information
        if (playerObject != null)
        {
            player = playerObject.transform;

            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    // Runs every frame while the game is playing
    void Update()
    {
        // Stop if the player or health system is missing
        if (player == null || playerHealth == null)
        {
            return;
        }

        // Measure the distance between enemy and player
        float distanceToPlayer = Vector2.Distance(
            transform.position,
            player.position
        );

        // Check whether the player is close enough
        bool playerInRange = distanceToPlayer <= attackRange;

        // Check whether the attack cooldown has finished
        bool canAttack = Time.time >= nextAttackTime;

        // Attack only when both conditions are true
        if (playerInRange && canAttack)
        {
            AttackPlayer();
        }
    }

    // Handles the enemy's attack
    void AttackPlayer()
    {
        // Deal damage using the player's existing health system
        playerHealth.TakeDamage(attackDamage);

        // Set the time when the enemy can attack again
        nextAttackTime = Time.time + attackCooldown;

        // Display attack information in Unity Console
        Debug.Log("Enemy attacked! Player took " +
                  attackDamage + " damage.");
    }
}
