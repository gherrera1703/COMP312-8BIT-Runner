using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private bool collected;
    private Vector3 restPosition;

    void Start() { restPosition = transform.position; }

    void Update()
    {
        transform.position = restPosition + Vector3.up *
            (Mathf.Sin(Time.time * 3f + restPosition.x) * 0.08f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        var progress = other.GetComponentInParent<PlayerProgress>();
        if (progress == null) return;
        collected = true;
        progress.AddCoin();
        FloatingFeedback.Show(transform.position, "+1", Color.yellow);
        gameObject.SetActive(false);
    }
}
