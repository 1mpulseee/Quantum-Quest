using UnityEngine;

public class ReactorEconomy : MonoBehaviour
{
    public static int BaseReactorCost = 1000; // Стоимость реактора

    public bool TryBuyReactor()
    {
        if (PlayerEconomy.Instance == null)
        {
            Debug.LogError("PlayerEconomy.Instance не найден!");
            return false;
        }

        if (PlayerEconomy.Instance.Money >= BaseReactorCost)
        {
            PlayerEconomy.Instance.SubtractMoney(BaseReactorCost); // Списываем деньги
            return true; // Покупка успешна
        }
        else
        {
            Debug.Log("Недостаточно денег для покупки реактора!");
            return false; // Покупка не удалась
        }
    }
}