using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(100)]
public class GameFlow : MonoBehaviour
{
    public TMP_FontAsset font;
    public bool IsPaused { get; private set; }
    public bool HasWon { get; private set; }
    private Canvas canvas;
    private GameObject page;
    private PlayerHealth health;
    private PlayerProgress progress;
    private LevelInfo level;
    private bool loading;
    private bool choosingClass;

    void Start()
    {
        Time.timeScale = 1f;
        level = FindAnyObjectByType<LevelInfo>();
        health = FindAnyObjectByType<PlayerHealth>();
        progress = FindAnyObjectByType<PlayerProgress>();
        if (font == null) font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        var root = new GameObject("Game Menus", typeof(RectTransform), typeof(Canvas),
            typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
        canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;
        var scaler = root.GetComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.matchWidthOrHeight = 0.5f;
        if (FindAnyObjectByType<EventSystem>() == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        if (health == null) ShowTitle();
        else
        {
            var gameOver = FindAnyObjectByType<GameOverController>();
            if (gameOver != null)
                Button(gameOver.panel.transform, "MAIN MENU", -205, GoToTitle);
            progress.Notify("LEVEL " + level.levelNumber + "  /  " + level.levelTitle, 3f);
            if (!ClassSelection.HasSelection) ShowClassSelection(level.levelNumber);
        }
    }

    void Update()
    {
        if (health == null || health.IsGameOver || HasWon || choosingClass) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else ShowPause();
        }
    }

    void NewPage(string heading, string subtitle = "")
    {
        if (page != null)
        {
            page.SetActive(false);
            Destroy(page);
        }
        page = new GameObject("Menu Page", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        page.transform.SetParent(canvas.transform, false);
        var rect = page.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        page.GetComponent<UnityEngine.UI.Image>().color = new Color(0.02f, 0.04f, 0.08f, 0.96f);
        Label(page.transform, heading, 220, 52, new Vector2(1100, 90), new Color(0.2f, 0.9f, 0.85f));
        if (!string.IsNullOrEmpty(subtitle)) Label(page.transform, subtitle, 125, 23, new Vector2(1100, 90), Color.white);
    }

    TMP_Text Label(Transform parent, string text, float y, float size, Vector2 dimensions, Color color)
    {
        var go = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        var r = go.GetComponent<RectTransform>();
        r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
        r.anchoredPosition = new Vector2(0, y);
        r.sizeDelta = dimensions;
        var label = go.GetComponent<TextMeshProUGUI>();
        label.font = font;
        label.fontSize = size;
        label.color = color;
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;
        label.text = text;
        return label;
    }

    void Button(Transform parent, string caption, float y, UnityEngine.Events.UnityAction action, bool enabled = true)
    {
        var go = new GameObject(caption, typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
        go.transform.SetParent(parent, false);
        var r = go.GetComponent<RectTransform>();
        r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
        r.anchoredPosition = new Vector2(0, y);
        r.sizeDelta = new Vector2(520, 58);
        var image = go.GetComponent<UnityEngine.UI.Image>();
        image.color = enabled ? new Color(0.15f, 0.8f, 0.75f) : new Color(0.22f, 0.26f, 0.32f);
        var button = go.GetComponent<UnityEngine.UI.Button>();
        button.targetGraphic = image;
        button.interactable = enabled;
        button.onClick.AddListener(action);
        Label(go.transform, caption, 0, 24, new Vector2(500, 54), enabled ? new Color(0.02f, 0.05f, 0.09f) : Color.gray);
    }

    public void ShowTitle()
    {
        NewPage("8-BIT RUNNER", "Three main levels. Three lives per level. One hidden challenge.");
        Button(page.transform, "PLAY", 35, () => LoadLevel(1));
        Button(page.transform, "LEVEL SELECT", -40, ShowLevelSelect);
        Button(page.transform, "CONTROLS", -115, ShowControls);
        Button(page.transform, "QUIT", -190, Quit);
    }

    public void ShowClassSelection(int targetLevel)
    {
        choosingClass = true;
        if (health != null) Time.timeScale = 0f;
        NewPage("CHOOSE YOUR CLASS", "One upgrade at half the coins in each level. Attack with J.");
        Button(page.transform, "RANGER - GUN / 20 DAMAGE", 45, () => ChooseClass(PlayerClass.Ranger, targetLevel));
        Button(page.transform, "FIGHTER - FISTS / 30 DAMAGE", -25, () => ChooseClass(PlayerClass.Fighter, targetLevel));
        Button(page.transform, "SWORDSMAN - SWORD / 30 DAMAGE", -95, () => ChooseClass(PlayerClass.Swordsman, targetLevel));
        Label(page.transform, "Ranger upgrade: faster shots, longer range, no reload\nFighter upgrade: faster punches, double knockback\nSwordsman upgrade: faster slashes, shockwaves", -205, 22, new Vector2(1000, 115), Color.white);
        Button(page.transform, "MAIN MENU", -270, GoToTitle);
    }

    void ChooseClass(PlayerClass choice, int targetLevel)
    {
        ClassSelection.Choose(choice);
        Load(CampaignProgress.Scenes[targetLevel - 1]);
    }

    public void ShowLevelSelect()
    {
        string coins = "Perfect clears: " + (CampaignProgress.IsPerfect(1) ? "1 " : "- ") +
            (CampaignProgress.IsPerfect(2) ? "2 " : "- ") + (CampaignProgress.IsPerfect(3) ? "3" : "-");
        NewPage("CHOOSE A LEVEL", coins + "\nCollect every coin AND finish each main level to unlock The Vault.");
        for (int i = 1; i <= 4; i++)
        {
            int chosen = i;
            bool unlocked = CampaignProgress.IsUnlocked(i);
            string caption = (i == 4 ? "SECRET: " : "LEVEL " + i + ": ") + CampaignProgress.Titles[i - 1];
            if (!unlocked) caption += "  [LOCKED]";
            else if (CampaignProgress.IsPerfect(i)) caption += "  [ALL COINS]";
            Button(page.transform, caption, 35 - (i - 1) * 70, () => LoadLevel(chosen), unlocked);
        }
        Button(page.transform, "MAIN MENU", -260, GoToTitle);
    }

    public void ShowControls()
    {
        NewPage("HOW TO PLAY");
        Label(page.transform,
            "A / D or arrows: move     SPACE: jump     J: class attack\nESC: pause     R: try again after Game Over\n\nCyan flags save checkpoints. Each death costs one life.\nHalf the coins = one class upgrade per level.\nUpgrades survive death; a new level or restart resets them.\n\nCollect every coin and finish all three main levels\nto unlock the secret level. Perfect clears are saved.",
            10, 25, new Vector2(1080, 310), Color.white);
        Button(page.transform, "BACK", -220, health == null ? (UnityEngine.Events.UnityAction)ShowTitle : ShowPause);
    }

    public void ShowPause()
    {
        if (health == null || health.IsGameOver || HasWon) return;
        IsPaused = true;
        Time.timeScale = 0f;
        NewPage("PAUSED", "LEVEL " + level.levelNumber + "  /  " + level.levelTitle);
        Button(page.transform, "RESUME", 35, Resume);
        Button(page.transform, "CONTROLS", -40, ShowControls);
        Button(page.transform, "RESTART LEVEL", -115, RestartLevel);
        Button(page.transform, "MAIN MENU", -190, GoToTitle);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        if (page != null) { page.SetActive(false); Destroy(page); }
    }

    public void CompleteLevel()
    {
        if (HasWon || health == null || health.IsGameOver) return;
        HasWon = true;
        IsPaused = false;
        CampaignProgress.RecordCompletion(level.levelNumber, progress.Coins, progress.TotalCoins);
        ShowVictoryScreen();
    }

    void ShowVictoryScreen()
    {
        Time.timeScale = 0f;
        string heading = level.levelNumber == 4 ? "THE VAULT COMPLETE!" : "LEVEL " + level.levelNumber + " COMPLETE!";
        string result = "Coins " + progress.Coins + "/" + progress.TotalCoins + "     Lives remaining " + health.LivesRemaining;
        result += progress.Coins >= progress.TotalCoins ? "\nPerfect clear saved!" : "\nReplay to find the missing coins.";
        if (level.levelNumber == 3)
            result += CampaignProgress.SecretUnlocked ? "\nTHE VAULT IS UNLOCKED!" : "\nThe Vault needs all coins from Levels 1, 2 and 3.";
        NewPage(heading, result);
        if (level.levelNumber < 3)
            Button(page.transform, "NEXT LEVEL", 20, () => LoadLevel(level.levelNumber + 1));
        else if (level.levelNumber == 3 && CampaignProgress.SecretUnlocked)
            Button(page.transform, "ENTER THE VAULT", 20, () => LoadLevel(4));
        else Button(page.transform, "LEVEL SELECT", 20, ShowLevelSelect);
        Button(page.transform, "REPLAY LEVEL", -60, RestartLevel);
        Button(page.transform, "MAIN MENU", -140, GoToTitle);
    }

    public void LoadLevel(int number)
    {
        if (number < 1 || number > 4 || !CampaignProgress.IsUnlocked(number)) return;
        if (health == null || !ClassSelection.HasSelection) { ShowClassSelection(number); return; }
        Load(CampaignProgress.Scenes[number - 1]);
    }
    public void RestartLevel() { Load(SceneManager.GetActiveScene().name); }
    public void GoToTitle() { Load("TitleScreen"); }
    void Load(string scene)
    {
        if (loading) return;
        loading = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void OnDestroy() { Time.timeScale = 1f; }
}
