using UnityEngine;

public static class CampaignProgress
{
    public static readonly string[] Scenes = { "MainLevel", "Level2", "Level3", "SecretLevel" };
    public static readonly string[] Titles = { "First Steps", "High Road", "Sky Siege", "The Vault" };
    private static string Key(int level, string kind) => "8BitRunner.Level" + level + "." + kind;

    public static bool IsComplete(int level) => PlayerPrefs.GetInt(Key(level, "Complete"), 0) == 1;
    public static bool IsPerfect(int level) => PlayerPrefs.GetInt(Key(level, "Perfect"), 0) == 1;
    public static bool SecretUnlocked => IsPerfect(1) && IsPerfect(2) && IsPerfect(3);
    public static bool IsUnlocked(int level) => level == 1 ||
        (level == 4 ? SecretUnlocked : level > 1 && level <= 3 && IsComplete(level - 1));

    public static void RecordCompletion(int level, int coins, int totalCoins)
    {
        if (level < 1 || level > 4) return;
        PlayerPrefs.SetInt(Key(level, "Complete"), 1);
        // Replaying with fewer coins never takes away an earned perfect clear.
        if (totalCoins > 0 && coins >= totalCoins)
            PlayerPrefs.SetInt(Key(level, "Perfect"), 1);
        PlayerPrefs.Save();
    }
}
