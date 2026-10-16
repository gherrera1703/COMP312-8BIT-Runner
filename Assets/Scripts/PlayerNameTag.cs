
using UnityEngine;
using TMPro;

public class PlayerNameTag : MonoBehaviour
{
    public string playerName = "Player1";

    private TextMeshPro nameText;

    void Awake()
    {
        nameText = GetComponent<TextMeshPro>();

        if (nameText != null)
        {
            nameText.text = playerName;
        }
    }

    void LateUpdate()
    {
        // Keep the name readable when the player turns
        Transform player = transform.parent;

        if (player != null)
        {
            float direction = Mathf.Sign(player.lossyScale.x);

            transform.localScale = new Vector3(
                0.1f * direction,
                0.1f,
                0.1f
            );
        }
    }
}
