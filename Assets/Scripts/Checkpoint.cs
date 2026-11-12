using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex = 1;
    public Vector2 respawnPosition;
    public SpriteRenderer flag;
    private bool activated;

    void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponentInParent<PlayerHealth>();
        if (health == null || activated) return;
        if (health.SetCheckpoint(checkpointIndex, respawnPosition))
        {
            activated = true;
            if (flag != null) flag.color = Color.green;
        }
    }
}
