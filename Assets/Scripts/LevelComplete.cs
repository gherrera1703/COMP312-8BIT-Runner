
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    // Tracks whether the level has been completed
    private bool levelFinished = false;

    // Runs when something touches the finish flag
    void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent the level from finishing multiple times
        if (levelFinished)
        {
            return;
        }

        // Check if the object touching the flag is the player
        if (other.gameObject.name == "Player")
        {
            // Mark the level as completed
            levelFinished = true;

            // Display a victory message in the Console
            Debug.Log("LEVEL COMPLETE!");

            // Stop the player's movement controls
            PlayerMovement playerMovement =
                other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // Stop the player's attack controls
            PlayerAttack playerAttack =
                other.GetComponent<PlayerAttack>();

            if (playerAttack != null)
            {
                playerAttack.enabled = false;
            }

            // Stop the player's horizontal movement
            Rigidbody2D playerRb =
                other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(
                    0f,
                    playerRb.linearVelocity.y
                );
            }

            // Stop all enemies when the level is completed
            StopEnemies();
        }
    }

    // Stops enemy movement and attacks
    void StopEnemies()
    {
        // Find all active enemies with patrol scripts
        EnemyPatrol[] enemies =
            FindObjectsByType<EnemyPatrol>(
                FindObjectsSortMode.None
            );

        // Go through each enemy in the level
        foreach (EnemyPatrol enemy in enemies)
        {
            // Disable enemy patrolling and chasing
            enemy.enabled = false;

            // Disable enemy attacks
            EnemyAttack attack =
                enemy.GetComponent<EnemyAttack>();

            if (attack != null)
            {
                attack.enabled = false;
            }

            // Stop horizontal enemy movement
            Rigidbody2D enemyRb =
                enemy.GetComponent<Rigidbody2D>();

            if (enemyRb != null)
            {
                enemyRb.linearVelocity = new Vector2(
                    0f,
                    enemyRb.linearVelocity.y
                );
            }
        }
    }

    // Displays the victory message on screen
    void OnGUI()
    {
        // Only display the message after winning
        if (levelFinished)
        {
            // Create the victory message style
            GUIStyle style = new GUIStyle(GUI.skin.label);

            style.fontSize = 40;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.yellow;

            // Display LEVEL COMPLETE at the top
            GUI.Label(
                new Rect(0, 60, Screen.width, 100),
                "LEVEL COMPLETE!",
                style
            );
        }
    }
}
