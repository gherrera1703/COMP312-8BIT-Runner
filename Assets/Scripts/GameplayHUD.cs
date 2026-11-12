using TMPro;
using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerProgress progress;
    public TMP_Text stats;
    public TMP_Text notification;
    private PlayerAttack attack;

    void Start() { if (health != null) attack = health.GetComponent<PlayerAttack>(); }

    void Update()
    {
        if (health == null || progress == null) return;
        if (stats != null)
            stats.text = "HEALTH " + health.currentHealth + "/" + health.maxHealth +
                         "    LIVES " + health.LivesRemaining +
                         "    COINS " + progress.Coins + "/" + progress.TotalCoins +
                         "    " + (health.CheckpointIndex == 0 ? "START" : "CHECKPOINT " + health.CheckpointIndex) +
                         (attack == null ? "" : "\n" + attack.Class.ToString().ToUpperInvariant() +
                         (attack.IsUpgraded ? "  LEVEL 2 - UPGRADED" : "  LEVEL 1 - UPGRADE AT " + progress.UpgradeThreshold + " COINS") +
                         (attack.Class != PlayerClass.Ranger ? "" : attack.IsUpgraded ? "    UNLIMITED AMMO" :
                         "    AMMO " + attack.RoundsRemaining + "/" + PlayerAttack.MagazineCapacity +
                         (attack.IsReloading ? " - RELOADING " + attack.ReloadRemaining.ToString("0.0") + "s" : "")));
        if (notification != null)
            notification.text = !health.IsGameOver && Time.time < progress.MessageUntil ? progress.Message : "";
    }
}
