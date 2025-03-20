using UnityEngine;
using UnityEngine.UI;

public class ReactorController : MonoBehaviour
{
    [Header("Temperature settings")]
    public float temperature = 500f; // Начальная температура
    public float optimalMin = 450f; // \
                                    //  > -  диапозон оптимально температуры
    public float optimalMax = 750f; // /
    public float coolingRate = 1f; // Скорость охлаждения
    public float heatingRate = 1f; // Скорость нагрева

    [SerializeField] private float _minTemp = 200f;
    [SerializeField] private float _maxTemp = 1000f;

    [Header("Economy")]
    public int reactorCost = 1000; // Стоимость реактора
    public float moneyInterval = 5f; // Интервал начисления денег (в секундах)
    public int moneyPerInterval = 10; // Количество денег за интервал

    [Header("UI")]
    [SerializeField] private Slider _temperatureSlider;
    [SerializeField] private MiniGameController _miniGameController;

    [Header("Effects")]
    [SerializeField] private GameObject _explosionEffect; // Префаб эффекта взрыва
    [SerializeField] private Renderer _reactorRenderer;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _overheatingColor = Color.red;
    [SerializeField] private Color _coolingColor = Color.blue;

    private float _timer;
    private bool _isMiniGameActive;

    private void Start()
    {
        _miniGameController.OnMiniGameEnded.AddListener(HandleMiniGameEnd);

        _reactorRenderer = GetComponent<Renderer>();
        _reactorRenderer.material.color = _normalColor;

        _timer = moneyInterval;
    }

    private void Update()
    {
        if (!_isMiniGameActive)
        {
            TemperatureChangeOverTime();

            _timer -= Time.deltaTime;
            IncomeCalculation();

            // Обновление индикатора температуры
            _temperatureSlider.value = temperature / _maxTemp;

            //BOOM
            if (temperature >= _maxTemp)
            {
                Explode();
            }

            UpdateReactorColor();
        }
    }

    public void UpTemperature(float amount) => temperature += amount;

    public void LowTemperatere(float amount)
    {
        if ((temperature -= amount) < _minTemp) StartMiniGame();
    }

    private void TemperatureChangeOverTime()
    {
        if (temperature > _minTemp && temperature < optimalMax)
            LowTemperatere(coolingRate * Time.deltaTime);
        else
            UpTemperature(heatingRate * Time.deltaTime);
    }

    /// <summary>
    /// Расчет доходов
    /// </summary>
    private void IncomeCalculation()
    {
        if (_timer <= 0)
        {
            if (temperature >= optimalMin && temperature <= optimalMax)
            {
                AddMoney(moneyPerInterval * 2);
                _timer = moneyInterval;
            }
            else
            {
                AddMoney(moneyPerInterval);
                _timer = moneyInterval;
            }
        }
    }

    private void AddMoney(int amount)
    {
        if (PlayerEconomy.Instance != null)
            PlayerEconomy.Instance.AddMoney(amount);
        else
            Debug.LogError("PlayerEconomy.Instance не найден!");
    }

    private void UpdateReactorColor()
    {
        if (temperature >= 750f) // Перегрев
        {
            float lerpValue = (temperature - 750f) / (_maxTemp - 750f);
            _reactorRenderer.material.color = Color.Lerp(_normalColor, _overheatingColor, lerpValue);
        }
        else if (temperature <= 450f) // Охлаждение
        {
            float lerpValue = (450f - temperature) / (450f - _minTemp);
            _reactorRenderer.material.color = Color.Lerp(_normalColor, _coolingColor, lerpValue);
        }
        else // Нормальный режим
        {
            _reactorRenderer.material.color = _normalColor;
        }
    }

    private void HandleMiniGameEnd(bool isWin)
    {
        if (!isWin)
            DestroyReactor();
        _isMiniGameActive = false;
    }

    /// <summary>
    /// Отписываемся от события при уничтожении реактора
    /// </summary>
    private void OnDestroy()
    {
        if (_miniGameController != null)
            _miniGameController.OnMiniGameEnded.RemoveListener(HandleMiniGameEnd);
    }

    private void DestroyReactor()
    {
        // Выплачиваем 50% от стоимости реактора
        int refundAmount = reactorCost / 2;
        AddMoney(refundAmount);

        Debug.Log($"Реактор уничтожен! Вы получили {refundAmount} денег.");
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Потом добавим взрыв!!!
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);  
        Debug.Log("Реактор взорвался!");
        Destroy(gameObject);
    }

    private void StartMiniGame()
    {
        _isMiniGameActive = true;
        _miniGameController.gameObject.SetActive(true);
        _miniGameController.StartMiniGame();
    }
}
