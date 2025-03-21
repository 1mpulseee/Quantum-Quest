using UnityEngine;

public class ReactorEconomy : MonoBehaviour
{
    public static int BaseReactorCost = 1000; // ��������� ��������

    public bool TryBuyReactor()
    {
        if (PlayerEconomy.Instance == null)
        {
            Debug.LogError("PlayerEconomy.Instance �� ������!");
            return false;
        }

        if (PlayerEconomy.Instance.Money >= BaseReactorCost)
        {
            PlayerEconomy.Instance.SubtractMoney(BaseReactorCost); // ��������� ������
            return true; // ������� �������
        }
        else
        {
            Debug.Log("������������ ����� ��� ������� ��������!");
            return false; // ������� �� �������
        }
    }
}