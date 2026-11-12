
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // How close the player must be for an attack
    public float attackRange = 1.5f;

    // How much damage each attack deals
    public int attackDamage = 10;

    // How long the enemy waits between attacks
    public float attackCooldown = 1.5f;

    [Header("Ranged attack (optional)")]
    public bool shootsProjectiles;
    public float projectileRange = 8f;
    public float projectileSpeed = 6f;
    public float projectileLifetime = 4f;
    public float warningDuration = 0.45f;
    public bool IsWindingUp { get; private set; }
    private float fireAt;
    private Vector2 aimedDirection;
    private SpriteRenderer enemyVisual;
    private Color originalColor;

    // Stores the player's position
    private Transform player;

    // Stores the player's health system
    private PlayerHealth playerHealth;

    // Tracks when the enemy can attack again
    private float nextAttackTime = 0f;

    // Runs once when the game starts
    void Start()
    {
        nextAttackTime = Time.time + attackCooldown;
        enemyVisual = GetComponent<SpriteRenderer>();
        if (enemyVisual != null) originalColor = enemyVisual.color;
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
        bool playerInRange = distanceToPlayer <=
            (shootsProjectiles ? projectileRange : attackRange);

        // Check whether the attack cooldown has finished
        bool canAttack = Time.time >= nextAttackTime;

        if (shootsProjectiles && IsWindingUp)
        {
            if (!playerInRange)
            {
                CancelWarning();
                nextAttackTime = Time.time + 0.25f;
                return;
            }
            if (enemyVisual != null)
                enemyVisual.color = Color.Lerp(originalColor, Color.white,
                    0.5f + 0.5f * Mathf.Sin(Time.time * 35f));
            if (Time.time >= fireAt)
            {
                AttackPlayer();
                CancelWarning();
            }
            return;
        }

        // Attack only when both conditions are true
        if (playerInRange && canAttack)
        {
            if (shootsProjectiles)
            {
                IsWindingUp = true;
                aimedDirection = (player.position - transform.position).normalized;
                fireAt = Time.time + Mathf.Max(0.1f, warningDuration);
            }
            else AttackPlayer();
        }
    }

    // Handles the enemy's attack
    void AttackPlayer()
    {
        if (shootsProjectiles)
        {
            Vector2 direction = IsWindingUp ? aimedDirection :
                (Vector2)(player.position - transform.position).normalized;
            if (direction.sqrMagnitude < 0.01f)
                direction = Vector2.right;

            var shot = new GameObject("Enemy Projectile");
            shot.transform.position = transform.position;
            shot.transform.localScale = Vector3.one * 0.28f;
            var visual = shot.AddComponent<SpriteRenderer>();
            visual.sprite = GetComponent<SpriteRenderer>().sprite;
            visual.color = new Color(1f, 0.45f, 0.05f);
            visual.sortingOrder = 10;
            shot.AddComponent<EnemyProjectile>().Initialize(
                direction, projectileSpeed, attackDamage, projectileLifetime);
            nextAttackTime = Time.time + attackCooldown;
            return;
        }

        // Deal damage using the player's existing health system
        playerHealth.TakeDamage(attackDamage);

        // Set the time when the enemy can attack again
        nextAttackTime = Time.time + attackCooldown;

        // Display attack information in Unity Console
        Debug.Log("Enemy attacked! Player took " +
                  attackDamage + " damage.");
    }
    void CancelWarning()
    {
        IsWindingUp = false;
        if (enemyVisual != null) enemyVisual.color = originalColor;
    }

    void OnDisable() { CancelWarning(); }
}
