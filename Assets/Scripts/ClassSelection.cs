using UnityEngine;

public enum PlayerClass { Ranger, Fighter, Swordsman }

public static class ClassSelection
{
    public static PlayerClass Selected { get; private set; } = PlayerClass.Fighter;
    public static bool HasSelection { get; private set; }

    public static void Choose(PlayerClass choice)
    {
        Selected = choice;
        HasSelection = true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetSession()
    {
        Selected = PlayerClass.Fighter;
        HasSelection = false;
    }
}
