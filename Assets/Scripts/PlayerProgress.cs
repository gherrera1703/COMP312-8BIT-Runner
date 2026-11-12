using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public int Coins { get; private set; }
    public int TotalCoins { get; private set; }
    public string Message { get; private set; }
    public float MessageUntil { get; private set; }
    public int UpgradeThreshold => Mathf.CeilToInt(TotalCoins * 0.5f);

    void Start()
    {
        TotalCoins = FindObjectsByType<CoinPickup>().Length;
    }

    public void AddCoin()
    {
        Coins++;
        if (TotalCoins > 0 && Coins >= UpgradeThreshold)
            GetComponent<PlayerAttack>()?.Upgrade();
    }

    public void Notify(string message, float seconds = 2f)
    {
        Message = message;
        MessageUntil = Time.time + seconds;
    }
}
