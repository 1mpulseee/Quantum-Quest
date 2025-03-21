using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
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

    public void CheckGameStateAfterReactorExplosion()
    {
        // ���������, ���� �� �������� �� �����
        GameObject[] reactors = GameObject.FindGameObjectsWithTag("Reactor"); 
        bool hasReactors = reactors.Length > 0;

        // ���������, ���������� �� ����� � ������ ��� ������ ��������
        bool hasEnoughMoney = PlayerEconomy.Instance.Money >= ReactorEconomy.BaseReactorCost;

        if (!hasReactors)
        {
            if (!hasEnoughMoney)
            {
                Debug.Log("��������: ��� ��������� � ������������ ����� ��� ������� ������.");
                // ������� � ��������� ���������
                GameState.SetState(new LoseState());
            }
            else
            {
                Debug.Log("���� ������������: ��� ���������, �� ���� ������ ��� ������� ������.");
            }
        }
        else if (hasReactors)
        {
            Debug.Log("���� ������������: �� ����� ���� ��������.");
        }
    }
}
public abstract class GameState
{
    public static void SetState(GameState newState)
    {
        newState.Handle();
    }

    public abstract void Handle();
}

public class LoseState : GameState
{
    public override void Handle()
    {
        Debug.Log("���� ��������: �� ���������!");
        // ����� ����� �������� ������ ��� ��������� (��������, �������� ����� ���������)
    }
}
