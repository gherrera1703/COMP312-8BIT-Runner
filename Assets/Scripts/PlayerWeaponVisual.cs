using UnityEngine;

public class PlayerWeaponVisual : MonoBehaviour
{
    private SpriteRenderer visual;
    private PlayerClass kind;
    private float swingUntil;
    private float duration;

    public void Initialize(Sprite sprite, PlayerClass weaponClass)
    {
        kind = weaponClass;
        visual = gameObject.AddComponent<SpriteRenderer>();
        visual.sprite = sprite;
        visual.sortingOrder = 12;
        if (kind == PlayerClass.Ranger)
        {
            transform.localPosition = new Vector3(0.65f, 0f, -0.1f);
            transform.localScale = new Vector3(0.7f, 0.22f, 1f);
            visual.color = new Color(0.3f, 0.85f, 1f);
        }
        else
        {
            transform.localPosition = new Vector3(kind == PlayerClass.Fighter ? 0.8f : 1.1f, 0f, -0.1f);
            transform.localScale = kind == PlayerClass.Fighter ? new Vector3(0.55f,0.45f,1f) : new Vector3(1.6f,0.15f,1f);
            visual.color = kind == PlayerClass.Fighter ? new Color(1f,0.7f,0.2f) : new Color(0.7f,1f,1f);
            visual.enabled = false;
        }
    }

    public void Swing(float seconds)
    {
        duration = seconds;
        swingUntil = Time.time + seconds;
        visual.enabled = true;
    }

    void Update()
    {
        if (kind == PlayerClass.Ranger) return;
        visual.enabled = Time.time < swingUntil;
        if (visual.enabled && kind == PlayerClass.Swordsman)
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-55f,55f,1f-(swingUntil-Time.time)/duration));
    }
}
