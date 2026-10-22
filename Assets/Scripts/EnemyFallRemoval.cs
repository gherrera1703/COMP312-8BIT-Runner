
using UnityEngine;

public class EnemyFallRemoval : MonoBehaviour
{
    // The lowest Y position the enemy can reach
    // before being removed from the game
    public float fallLimit = -10f;

    // Runs every frame while the game is playing
    void Update()
    {
        // Check if the enemy has fallen below the level
        if (transform.position.y < fallLimit)
        {
            // Display a message in the Unity Console
            Debug.Log("Enemy fell off the map!");

            // Remove the enemy from the game
            Destroy(gameObject);
        }
    }
}
