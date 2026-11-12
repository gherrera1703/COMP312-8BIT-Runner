using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerProgress progress;
    public GameObject panel;
    public TMP_Text summary;
    public UnityEngine.UI.Button restartButton;
    private bool showing;
    private bool restarting;

    void Start()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(RestartRun);
    }

    void Update()
    {
        if (health == null || !health.IsGameOver) return;
        Show();
        if (Input.GetKeyDown(KeyCode.R)) RestartRun();
    }

    public void Show()
    {
        if (showing) return;
        showing = true;
        summary.text = "No lives left\nCoins collected: " + progress.Coins + "/" + progress.TotalCoins;
        panel.SetActive(true);
        foreach (var attack in FindObjectsByType<EnemyAttack>()) attack.enabled = false;
        foreach (var patrol in FindObjectsByType<EnemyPatrol>())
        {
            patrol.enabled = false;
            var body = patrol.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                body.linearVelocity = Vector2.zero;
                body.simulated = false;
            }
        }
        foreach (var shot in FindObjectsByType<EnemyProjectile>()) Destroy(shot.gameObject);
        foreach (var shot in FindObjectsByType<PlayerProjectile>()) Destroy(shot.gameObject);
        foreach (var follow in FindObjectsByType<CameraFollow>()) follow.enabled = false;
    }

    public void RestartRun()
    {
        if (restarting || health == null || !health.IsGameOver) return;
        restarting = true;
        SceneManager.LoadScene(gameObject.scene.path);
    }

    void OnDestroy()
    {
        if (restartButton != null) restartButton.onClick.RemoveListener(RestartRun);
    }
}
