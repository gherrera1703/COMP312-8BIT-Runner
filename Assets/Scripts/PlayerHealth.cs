
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float respawnX = 0f;
    public float respawnY = 0f;

    public float fallLimit = -10f;

    [Header("Lives")]
    [Min(1)] public int startingLives = 3;
    public int LivesRemaining { get; private set; }
    public bool IsGameOver { get; private set; }

    [Header("Damage feedback")]
    public float invincibilityDuration = 0.8f;
    public float respawnProtectionDuration = 1.2f;
    public float damageFlashDuration = 0.15f;

    public int CheckpointIndex { get; private set; }
    public bool IsInvincible => Time.time < invincibleUntil;

    private Rigidbody2D rb;
    private SpriteRenderer visual;
    private Color originalColor;
    private float invincibleUntil;
    private float flashUntil;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        visual = GetComponent<SpriteRenderer>();
        if (visual != null) originalColor = visual.color;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Respawn where the player begins this level.
        respawnX = transform.position.x;
        respawnY = transform.position.y;
        currentHealth = maxHealth;
        LivesRemaining = Mathf.Max(1, startingLives);

        Debug.Log("Player Health: " + currentHealth);
    }

    void Update()
    {
        if (IsGameOver) return;
        if (transform.position.y < fallLimit)
        {
            // Falling always kills, even during damage protection.
            Die();
        }

        if (visual == null) return;
        if (Time.time < flashUntil)
            visual.color = Color.red;
        else if (IsInvincible)
        {
            Color blinking = originalColor;
            blinking.a *= Mathf.Sin(Time.time * 30f) > 0f ? 0.35f : 1f;
            visual.color = blinking;
        }
        else visual.color = originalColor;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || IsInvincible || IsGameOver) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        invincibleUntil = Time.time + invincibilityDuration;
        flashUntil = Time.time + damageFlashDuration;
        FloatingFeedback.Show(transform.position + Vector3.up, "-" + damage, Color.red);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (IsGameOver) return;
        LivesRemaining = Mathf.Max(0, LivesRemaining - 1);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (LivesRemaining == 0)
        {
            IsGameOver = true;
            currentHealth = 0;
            rb.simulated = false;
            var controls = GetComponent<PlayerMovement>();
            if (controls != null) controls.enabled = false;
            var attack = GetComponent<PlayerAttack>();
            if (attack != null) attack.enabled = false;
            if (visual != null) visual.color = Color.red;
            var gameOver = FindAnyObjectByType<GameOverController>();
            if (gameOver != null) gameOver.Show();
            Debug.Log("GAME OVER: no lives left.");
            return;
        }

        Debug.Log("Player died! Lives remaining: " + LivesRemaining);

        transform.position = new Vector3(
            respawnX,
            respawnY,
            transform.position.z
        );

        currentHealth = maxHealth;
        invincibleUntil = Time.time + respawnProtectionDuration;
        flashUntil = 0f;
        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.ResetAfterRespawn();
        GetComponent<PlayerAttack>()?.ResetMagazine();
        var progress = GetComponent<PlayerProgress>();
        if (progress != null) progress.Notify(CheckpointIndex > 0 ? "Back to checkpoint" : "Back to the start");

        Debug.Log("Player respawned! Health: " + currentHealth);
    }

    public bool SetCheckpoint(int index, Vector2 position)
    {
        if (IsGameOver || index <= CheckpointIndex || position.y <= fallLimit) return false;
        CheckpointIndex = index;
        respawnX = position.x;
        respawnY = position.y;
        var progress = GetComponent<PlayerProgress>();
        if (progress != null) progress.Notify("Checkpoint " + index + " saved!");
        return true;
    }

    void OnDisable()
    {
        if (visual != null) visual.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "DamageHazard")
        {
            TakeDamage(25);
        }
    }
}

