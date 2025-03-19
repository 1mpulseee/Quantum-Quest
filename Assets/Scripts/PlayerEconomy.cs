using UnityEngine;
using TMPro;

public class PlayerEconomy : MonoBehaviour
{
    public static PlayerEconomy Instance;

    [SerializeField] private TextMeshProUGUI _moneyText;

    public int Money { get; private set; } 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        UpdateMoneyUI();
    }

    public void SubtractMoney(int amount)
    {
        Money -= amount;
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
