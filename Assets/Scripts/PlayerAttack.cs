using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.J;
    public const int MagazineCapacity = 10;
    public const float ReloadDuration = 5f;
    public int RoundsRemaining { get; private set; }
    public bool IsReloading { get; private set; }
    public float ReloadRemaining => IsReloading ? Mathf.Max(0f, reloadEndsAt - Time.time) : 0f;
    private float reloadEndsAt;
    public bool IsUpgraded { get; private set; }
    public PlayerClass Class { get; private set; }
    public int Damage => Class == PlayerClass.Ranger ? 20 : 30;
    public float Cooldown => Class == PlayerClass.Ranger ? (IsUpgraded ? 0.28f : 0.5f) :
        Class == PlayerClass.Fighter ? (IsUpgraded ? 0.27f : 0.45f) : (IsUpgraded ? 0.36f : 0.6f);
    public float Range => Class == PlayerClass.Ranger ? (IsUpgraded ? 12f : 8f) :
        Class == PlayerClass.Fighter ? 1.2f : 2.2f;
    public float KnockbackMultiplier => Class == PlayerClass.Fighter && IsUpgraded ? 2f : 1f;
    private float nextAttackTime;
    private Sprite sprite;
    private PlayerWeaponVisual weapon;

    void Start() { ApplyClass(ClassSelection.Selected); }

    public void ApplyClass(PlayerClass choice)
    {
        Class = choice;
        IsUpgraded = false;
        nextAttackTime = 0f;
        ResetMagazine();
        sprite = GetComponent<SpriteRenderer>().sprite;
        if (weapon != null) Destroy(weapon.gameObject);
        var go = new GameObject("Class Weapon");
        go.transform.SetParent(transform, false);
        weapon = go.AddComponent<PlayerWeaponVisual>();
        weapon.Initialize(sprite, Class);
    }

    void Update()
    {
        RefreshReload();
        if (Input.GetKeyDown(attackKey)) TryAttack();
    }

    void RefreshReload()
    {
        if (IsReloading && Time.time >= reloadEndsAt)
        {
            RoundsRemaining = MagazineCapacity;
            IsReloading = false;
        }
    }

    public void ResetMagazine()
    {
        RoundsRemaining = MagazineCapacity;
        IsReloading = false;
        reloadEndsAt = 0f;
        nextAttackTime = 0f;
    }

    public bool TryAttack()
    {
        if (!enabled || Time.timeScale <= 0f || !ClassSelection.HasSelection || Time.time < nextAttackTime)
            return false;
        var health = GetComponent<PlayerHealth>();
        if (health != null && health.IsGameOver) return false;
        RefreshReload();
        if (Class == PlayerClass.Ranger && !IsUpgraded && IsReloading) return false;
        nextAttackTime = Time.time + Cooldown;
        float direction = transform.localScale.x >= 0f ? 1f : -1f;
        if (Class == PlayerClass.Ranger)
        {
            Projectile(direction, false, null);
            if (!IsUpgraded) RoundsRemaining--;
            if (!IsUpgraded && RoundsRemaining == 0)
            {
                IsReloading = true;
                reloadEndsAt = Time.time + ReloadDuration;
            }
            return true;
        }

        weapon.Swing(0.16f);
        float center = Class == PlayerClass.Fighter ? 0.75f : 1.25f;
        Vector2 size = new Vector2(Class == PlayerClass.Fighter ? 0.9f : 1.9f, 0.8f);
        Vector2 position = (Vector2)transform.position + Vector2.right * direction * center;
        var hitEnemies = new HashSet<EnemyHealth>();
        foreach (var hit in Physics2D.OverlapBoxAll(position, size, 0f))
        {
            var enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy != null && hitEnemies.Add(enemy))
                enemy.TakeDamage(Damage, KnockbackMultiplier);
        }
        if (Class == PlayerClass.Swordsman && IsUpgraded)
            Projectile(direction, true, hitEnemies);
        return true;
    }

    void Projectile(float direction, bool wave, IEnumerable<EnemyHealth> alreadyHit)
    {
        var shot = new GameObject(wave ? "Sword Shockwave" : "Ranger Pellet");
        shot.transform.position = transform.position + new Vector3(direction * 0.65f, 0f, -0.15f);
        shot.transform.localScale = wave ? new Vector3(0.22f, 0.85f, 1f) : new Vector3(0.3f, 0.12f, 1f);
        var visual = shot.AddComponent<SpriteRenderer>();
        visual.sprite = sprite;
        visual.color = wave ? Color.cyan : Color.yellow;
        visual.sortingOrder = 10;
        shot.AddComponent<PlayerProjectile>().Initialize(Vector2.right * direction,
            wave ? 8f : 10f, wave ? 5f : Range, Damage, wave, alreadyHit);
    }

    public bool Upgrade()
    {
        if (IsUpgraded) return false;
        IsUpgraded = true;
        // Leveling up immediately cancels any reload already in progress.
        if (Class == PlayerClass.Ranger) ResetMagazine();
        GetComponent<PlayerProgress>()?.Notify(Class + " LEVEL UP!", 4f);
        FloatingFeedback.ShowLevelUp(transform);
        return true;
    }
}
