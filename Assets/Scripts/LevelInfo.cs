using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Range(1, 4)] public int levelNumber = 1;
    public string levelTitle = "First Steps";
}
