using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReactorController : MonoBehaviour
{
    public UnityEvent OnReactorDestroyed; // Событие удаления реактора

    [Header("Temperature Settings")]
    public float temperature = 500f; // Начальная температура
    public float optimalMin = 450f; // Минимальная оптимальная температура
    public float optimalMax = 750f; // Максимальная оптимальная температура
    public float coolingRate = 1f; // Скорость охлаждения
    public float heatingRate = 1f; // Скорость нагрева

    [SerializeField] private float _minTemp = 200f;
    [SerializeField] private float _maxTemp = 1000f;

    [Header("Economy")]
    public float moneyInterval = 5f; // Интервал начисления денег (в секундах)
    public int moneyPerInterval = 10; // Количество денег за интервал

    [Header("UI")]
    [SerializeField] private Slider _temperatureSlider;

    [Header("Effects")]
    [SerializeField] private GameObject _explosionEffect; // Префаб эффекта взрыва
    [SerializeField] private Renderer _reactorRenderer;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _overheatingColor = Color.red;
    [SerializeField] private Color _coolingColor = Color.blue;

    private float _timer;
    private bool _isMiniGameActive;
    private MiniGameController _miniGameController;

    private void Start()
    {
        if (_reactorRenderer == null)
            _reactorRenderer = GetComponent<Renderer>();

        if (_reactorRenderer == null)
            Debug.LogError("Renderer не найден на объекте реактора!");
        else
            _reactorRenderer.material.color = _normalColor;

        _miniGameController = FindMiniGameController();
        if (_miniGameController == null)
            Debug.LogError("MiniGameController не найден в сцене!");
        else
            _miniGameController.OnMiniGameEnded.AddListener(HandleMiniGameEnd);

        _timer = moneyInterval;
    }

    private MiniGameController FindMiniGameController()
    {
        MiniGameController[] controllers = Resources.FindObjectsOfTypeAll<MiniGameController>();
        return controllers.Length > 0 ? controllers[0] : null;
    }

    private void Update()
    {
        if (!_isMiniGameActive)
        {
            TemperatureChangeOverTime();
            UpdateIncome();
            UpdateTemperatureUI();
            CheckForExplosion();
            UpdateReactorColor();
        }
    }

    public void UpTemperature(float amount) => temperature += amount;

    public void LowTemperature(float amount)
    {
        if ((temperature -= amount) < _minTemp) StartMiniGame();
    }

    public void DestroyReactor()
    {
        int refundAmount = ReactorEconomy.BaseReactorCost / 2; // Возвращаем 50% стоимости (пример)
        AddMoney(refundAmount);

        Debug.Log($"Реактор уничтожен! Вы получили {refundAmount} денег.");

        DeactivateReactor();
    }

    public void Explode()
    {
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        Debug.Log("Реактор взорвался!");

        DeactivateReactor();
    }

    private void DeactivateReactor()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 3f);
        OnReactorDestroyed?.Invoke();
        GameManager.Instance.CheckGameStateAfterReactorExplosion();
    }

    private void TemperatureChangeOverTime()
    {
        if (temperature > _minTemp && temperature < optimalMax)
            LowTemperature(coolingRate * Time.deltaTime);
        else
            UpTemperature(heatingRate * Time.deltaTime);
    }

    private void UpdateIncome()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            int money = temperature >= optimalMin && temperature <= optimalMax ? moneyPerInterval * 2 : moneyPerInterval;
            AddMoney(money);
            _timer = moneyInterval;
        }
    }

    private void AddMoney(int amount)
    {
        if (PlayerEconomy.Instance != null)
            PlayerEconomy.Instance.AddMoney(amount);
        else
            Debug.LogError("PlayerEconomy.Instance не найден!");
    }

    private void UpdateTemperatureUI()
    {
        if (_temperatureSlider != null)
            _temperatureSlider.value = temperature / _maxTemp;
    }

    private void CheckForExplosion()
    {
        if (temperature >= _maxTemp)
            Explode();
    }

    private void UpdateReactorColor()
    {
        if (_reactorRenderer == null) return;

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

    private void OnDestroy()
    {
        if (_miniGameController != null)
            _miniGameController.OnMiniGameEnded.RemoveListener(HandleMiniGameEnd);

        OnReactorDestroyed?.Invoke();
    }

    private void StartMiniGame()
    {
        if (_miniGameController == null)
        {
            Debug.LogError("MiniGameController не назначен!");
            return;
        }

        _isMiniGameActive = true;
        _miniGameController.gameObject.SetActive(true);
        _miniGameController.StartMiniGame();
    }
}