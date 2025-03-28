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
        // Проверяем, есть ли реакторы на сцене
        GameObject[] reactors = GameObject.FindGameObjectsWithTag("Reactor"); 
        bool hasReactors = reactors.Length > 0;

        // Проверяем, достаточно ли денег у игрока для нового реактора
        bool hasEnoughMoney = PlayerEconomy.Instance.Money >= ReactorEconomy.BaseReactorCost;

        if (!hasReactors)
        {
            if (!hasEnoughMoney)
            {
                Debug.Log("Проигрыш: Нет реакторов и недостаточно денег для покупки нового.");
                // Переход в состояние проигрыша
                GameState.SetState(new LoseState());
            }
            else
            {
                Debug.Log("Игра продолжается: Нет реакторов, но есть деньги для покупки нового.");
            }
        }
        else if (hasReactors)
        {
            Debug.Log("Игра продолжается: На сцене есть реакторы.");
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
        Debug.Log("Игра окончена: Вы проиграли!");
        // Здесь можно добавить логику для проигрыша (например, показать экран поражения)
    }
}
