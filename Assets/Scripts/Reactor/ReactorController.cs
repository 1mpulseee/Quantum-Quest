using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReactorController : MonoBehaviour
{
    public UnityEvent OnReactorDestroyed; // ������� �������� ��������

    [Header("Temperature Settings")] public float temperature = 500f; // ��������� �����������
    public float optimalMin = 450f; // ����������� ����������� �����������
    public float optimalMax = 750f; // ������������ ����������� �����������
    public float coolingRate = 1f; // �������� ����������
    public float heatingRate = 1f; // �������� �������

    [SerializeField] private float _minTemp = 200f;
    [SerializeField] private float _maxTemp = 1000f;

    [Header("Economy")] public float moneyInterval = 5f; // �������� ���������� ����� (� ��������)
    public int moneyPerInterval = 10; // ���������� ����� �� ��������

    [Header("UI")][SerializeField] private Slider _temperatureSlider;

    [Header("Effects")][SerializeField] private GameObject _explosionEffect; // ������ ������� ������
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
            Debug.LogError("Renderer �� ������ �� ������� ��������!");
        else
            _reactorRenderer.material.color = _normalColor;

        _miniGameController = FindMiniGameController();
        if (_miniGameController == null)
            Debug.LogError("MiniGameController �� ������ � �����!");
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
        if ((temperature -= amount) < _minTemp)
            StartMiniGame();
    }

    public void DestroyReactor()
    {
        // Находим все реакторы на сцене (используем современный метод FindObjectsByType)
        ReactorController[] allReactors = FindObjectsByType<ReactorController>(FindObjectsSortMode.None);

        if (allReactors.Length > 0)
        {
            // Ищем реактор с минимальной температурой
            ReactorController coldestReactor = allReactors[0];
            foreach (var reactor in allReactors)
            {
                if (reactor.temperature < coldestReactor.temperature)
                {
                    coldestReactor = reactor;
                }
            }

            // Деактивируем найденный реактор
            int refundAmount = ReactorEconomy.BaseReactorCost / 2;
            AddMoney(refundAmount);
            Debug.Log($"Уничтожен реактор с температурой {coldestReactor.temperature}! Вы получили {refundAmount} денег.");

            coldestReactor.DeactivateReactor();
            return; // Выходим, так как нашли реактор для деактивации
        }
    }

    public void Explode()
    {
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        Debug.Log("������� ���������!");

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
            int money = temperature >= optimalMin && temperature <= optimalMax
                ? moneyPerInterval * 2
                : moneyPerInterval;
            AddMoney(money);
            _timer = moneyInterval;
        }
    }

    private void AddMoney(int amount)
    {
        if (PlayerEconomy.Instance != null)
            PlayerEconomy.Instance.AddMoney(amount);
        else
            Debug.LogError("PlayerEconomy.Instance �� ������!");
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

        if (temperature >= 750f) // ��������
        {
            float lerpValue = (temperature - 750f) / (_maxTemp - 750f);
            _reactorRenderer.material.color = Color.Lerp(_normalColor, _overheatingColor, lerpValue);
        }
        else if (temperature <= 450f) // ����������
        {
            float lerpValue = (450f - temperature) / (450f - _minTemp);
            _reactorRenderer.material.color = Color.Lerp(_normalColor, _coolingColor, lerpValue);
        }
        else // ���������� �����
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
            Debug.LogError("MiniGameController �� ��������!");
            return;
        }

        _isMiniGameActive = true;
        _miniGameController.gameObject.SetActive(true);
        _miniGameController.StartMiniGame();
    }
}