using UnityEngine;
using TMPro;

public class PlayerEconomy : MonoBehaviour
{
    public static PlayerEconomy Instance { get; private set; }

    [SerializeField] private int _playerMoney = 1000;
    [SerializeField] private TextMeshProUGUI _moneyText;

    public int Money { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Money = _playerMoney;
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        UpdateMoneyUI();
    }

    public void SubtractMoney(int amount)
    {
        Money -= amount;
        UpdateMoneyUI();
        Debug.Log($"Списано {amount} денег. Текущий баланс: {Money}");
    }

    private void UpdateMoneyUI()
    {
        if (_moneyText != null)
        {
            _moneyText.text = $"Деньги: {Money}";
        }
    }
}
