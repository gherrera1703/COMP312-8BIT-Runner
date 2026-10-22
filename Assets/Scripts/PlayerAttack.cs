
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.J;
    public float attackRange = 0.8f;
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.4f;
    public int attackDamage = 25;

    private float nextAttackTime;

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        // Detect which direction the player is facing
        float direction = Mathf.Sign(transform.localScale.x);

        Vector2 attackPosition = new Vector2(
            transform.position.x + direction * attackRange,
            transform.position.y
        );

        // Check for enemies within attack range
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPosition,
            new Vector2(0.7f, 0.7f),
            0f
        );

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        // Create the temporary red attack box
        GameObject attackBox = new GameObject("AttackBox");

        attackBox.transform.position = new Vector3(
            attackPosition.x,
            attackPosition.y,
            -0.2f
        );

        attackBox.transform.localScale = new Vector3(
            0.7f,
            0.7f,
            1f
        );

        SpriteRenderer spriteRenderer =
            attackBox.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = CreateSquareSprite();
        spriteRenderer.color = Color.red;
        spriteRenderer.sortingOrder = 10;

        Destroy(attackBox, attackDuration);

        Debug.Log("Player attacked!");
    }

    Sprite CreateSquareSprite()
    {
        Texture2D texture = Texture2D.whiteTexture;

        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            1f
        );
    }
}
