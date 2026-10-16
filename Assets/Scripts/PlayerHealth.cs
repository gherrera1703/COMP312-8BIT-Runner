
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float respawnX = 0f;
    public float respawnY = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        Debug.Log("Player Health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died! Respawning...");

        rb.linearVelocity = Vector2.zero;

        transform.position = new Vector3(
            respawnX,
            respawnY,
            transform.position.z
        );

        currentHealth = maxHealth;

        Debug.Log("Player respawned! Health: " + currentHealth);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "DamageHazard")
        {
            TakeDamage(25);
        }
    }
}

