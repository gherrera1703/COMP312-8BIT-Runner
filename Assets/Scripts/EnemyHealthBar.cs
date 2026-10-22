
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public Transform healthBarFill;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = healthBarFill.localScale;
        originalPosition = healthBarFill.localPosition;
    }

    void Update()
    {
        if (enemyHealth == null || healthBarFill == null)
        {
            return;
        }

        float healthPercent = Mathf.Clamp01(
            (float)enemyHealth.currentHealth / enemyHealth.maxHealth
        );

        healthBarFill.localScale = new Vector3(
            originalScale.x * healthPercent,
            originalScale.y,
            originalScale.z
        );

        healthBarFill.localPosition = new Vector3(
            originalPosition.x -
            (originalScale.x * (1f - healthPercent) / 2f),
            originalPosition.y,
            originalPosition.z
        );
    }
}
