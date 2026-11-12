using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private bool spent;
    private const float Radius = 0.14f;

    public void Initialize(Vector2 travelDirection, float travelSpeed, int hitDamage, float lifetime)
    {
        direction = travelDirection.normalized;
        speed = Mathf.Max(0f, travelSpeed);
        damage = Mathf.Max(0, hitDamage);
        Destroy(gameObject, Mathf.Max(0.1f, lifetime));
    }

    void FixedUpdate()
    {
        if (spent) return;

        float distance = speed * Time.fixedDeltaTime;
        // Sweep the whole step so a fast shot cannot pass through a thin platform.
        var hits = Physics2D.CircleCastAll(transform.position, Radius, direction, distance);
        foreach (var hit in hits)
        {
            var target = hit.collider;
            if (target.GetComponentInParent<EnemyHealth>() != null)
                continue;

            var player = target.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                spent = true;
                player.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            // Trigger zones do not block shots, but solid level geometry does.
            if (!target.isTrigger)
            {
                spent = true;
                Destroy(gameObject);
                return;
            }
        }

        transform.position += (Vector3)(direction * distance);
    }
}
