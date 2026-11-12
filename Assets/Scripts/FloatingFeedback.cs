using TMPro;
using UnityEngine;

public class FloatingFeedback : MonoBehaviour
{
    private TextMeshPro label;
    private float age;
    private Color color;
    private float lifetime = 0.7f;
    private Transform follow;
    private Vector3 followOffset;

    public static void ShowLevelUp(Transform character)
    {
        Show(character.position + Vector3.up * 1.8f, "LEVEL UP!", Color.cyan, 2f, 6f, character);
    }

    public static void Show(Vector3 position, string message, Color tint,
        float seconds = 0.7f, float fontSize = 4f, Transform target = null)
    {
        var popup = new GameObject(target != null ? "Level Up Feedback" : "Hit Feedback");
        popup.transform.position = position + Vector3.back * 0.2f;
        var text = popup.AddComponent<TextMeshPro>();
        text.text = message;
        text.fontSize = fontSize;
        text.fontStyle = target != null ? FontStyles.Bold : FontStyles.Normal;
        text.alignment = TextAlignmentOptions.Center;
        text.color = tint;
        text.outlineWidth = 0.15f;
        text.sortingOrder = 50;
        text.rectTransform.sizeDelta = new Vector2(target != null ? 6f : 3f, 1.5f);
        var effect = popup.AddComponent<FloatingFeedback>();
        effect.label = text;
        effect.color = tint;
        effect.lifetime = seconds;
        effect.follow = target;
        if (target != null) effect.followOffset = popup.transform.position - target.position;
        Destroy(popup, seconds);
    }

    void Update()
    {
        age += Time.deltaTime;
        if (follow != null)
            transform.position = follow.position + followOffset + Vector3.up * (age * 0.2f);
        else transform.position += Vector3.up * (Time.deltaTime * 0.9f);
        float fadeStart = lifetime * 0.65f;
        color.a = 1f - Mathf.Clamp01((age - fadeStart) / (lifetime - fadeStart));
        label.color = color;
    }
}
