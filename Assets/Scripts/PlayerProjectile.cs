using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float remainingDistance;
    private int damage;
    private bool piercing;
    private bool spent;
    private readonly HashSet<EnemyHealth> hitEnemies = new HashSet<EnemyHealth>();

    public void Initialize(Vector2 heading, float travelSpeed, float range, int hitDamage,
        bool isShockwave, IEnumerable<EnemyHealth> alreadyHit = null)
    {
        direction = heading.normalized;
        speed = travelSpeed;
        remainingDistance = range;
        damage = hitDamage;
        piercing = isShockwave;
        if (alreadyHit != null) foreach (var enemy in alreadyHit) hitEnemies.Add(enemy);
        Destroy(gameObject, range / Mathf.Max(0.1f, speed) + 0.2f);
    }

    void FixedUpdate()
    {
        if (spent) return;
        float step = Mathf.Min(speed * Time.fixedDeltaTime, remainingDistance);
        foreach (var hit in Physics2D.CircleCastAll(transform.position, piercing ? 0.28f : 0.09f, direction, step))
        {
            var collider = hit.collider;
            if (collider.GetComponentInParent<PlayerHealth>() != null) continue;
            var enemy = collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                if (!hitEnemies.Add(enemy)) continue;
                enemy.TakeDamage(damage);
                if (!piercing) { Stop(); return; }
            }
            else if (!collider.isTrigger) { Stop(); return; }
        }
        transform.position += (Vector3)(direction * step);
        remainingDistance -= step;
        if (remainingDistance <= 0f) Stop();
    }

    void Stop() { spent = true; Destroy(gameObject); }
}
