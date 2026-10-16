
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Transform healthBarFill;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = healthBarFill.localScale;
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (playerHealth == null || healthBarFill == null)
            return;

        float healthPercent = (float)playerHealth.currentHealth
                            / playerHealth.maxHealth;

        healthPercent = Mathf.Clamp01(healthPercent);

        healthBarFill.localScale = new Vector3(
            originalScale.x * healthPercent,
            originalScale.y,
            originalScale.z
        );

        healthBarFill.localPosition = new Vector3(
            -(1f - healthPercent) / 2f,
            0f,
            -0.1f
        );
    }
}
