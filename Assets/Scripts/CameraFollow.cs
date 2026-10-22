
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player that the camera will follow
    public Transform target;

    // How smoothly the camera moves
    public float smoothSpeed = 5f;

    // Controls the camera's position relative to the player
    public Vector3 offset = new Vector3(0f, 1f, -10f);

    // Runs after the player's movement each frame
    void LateUpdate()
    {
        // Stop if no player has been assigned
        if (target == null)
        {
            return;
        }

        // Calculate where the camera should move
        Vector3 desiredPosition = target.position + offset;

        // Move the camera smoothly toward the player
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
