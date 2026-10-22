
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
        }
    }
}
