using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;

public class MiniGameController : MonoBehaviour
{
    public UnityEvent<bool> OnMiniGameEnded; // Событие окончания мини-игры

    public List<Image> lamps;
    public List<Image> diodes;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _diodeDelay = 1f; // Задержка между переключениями диодов
    [SerializeField] private float _gameDuration = 30f; // Длительность мини-игры в секундах

    private List<ReactorController> _reactors; // Список всех реакторов
    private int _currentDiodeIndex; // Индекс текущего активного диода
    private bool _isGameActive; // Активна ли мини-игра
    private float _timeRemaining; // Оставшееся время
    private bool _isProcessingClick; // Флаг для обработки клика
    private bool _isClockwise = true; // Направление движения диодов (по часовой стрелке)

    private void Start()
    {
        // Находим все реакторы в сцене
        _reactors = new List<ReactorController>(FindObjectsByType<ReactorController>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        ResetMiniGame();
    }

    private void Update()
    {
        if (_isGameActive)
        {
            UpdateTimer();
            CheckForInput();
            ExitMiniGame();
        }
    }

    public void StartMiniGame()
    {
        gameObject.SetActive(true);

        if (_isGameActive) return;

        _isProcessingClick = false;
        _isGameActive = true;
        _timeRemaining = _gameDuration;
        _currentDiodeIndex = 0;
        _isClockwise = true; // Начинаем с движения по часовой стрелке

        // Сбрасываем состояние лампочек и диодов
        ResetMiniGame();

        StartCoroutine(ActivateDiodes()); // Запускаем переключение диодов
        Debug.Log("Мини-игра запущена!");
    }

    public void EndMiniGame(bool isWin)
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        StopAllCoroutines(); // Останавливаем переключение диодов

        if (isWin)
        {
            Debug.Log("Мини-игра выиграна!");

            // Находим реактор с наименьшей температурой
            ReactorController coldestReactor = GetColdestReactor();
            if (coldestReactor != null)
                coldestReactor.temperature = 450f; // Устанавливаем температуру
        }
        else
        {
            Debug.Log("Мини-игра проиграна!");
        }

        OnMiniGameEnded.Invoke(isWin); // Уведомляем о завершении мини-игры
        gameObject.SetActive(false); // Скрываем мини-игру
    }

    private ReactorController GetColdestReactor()
    {
        if (_reactors == null || _reactors.Count == 0)
            return null;

        ReactorController coldestReactor = _reactors[0];
        foreach (var reactor in _reactors)
        {
            if (reactor.temperature < coldestReactor.temperature)
            {
                coldestReactor = reactor;
            }
        }
        return coldestReactor;
    }

    private void ExitMiniGame()
    {
        if (Input.GetKeyDown(KeyCode.F))
            EndMiniGame(false);
    }

    private void UpdateTimer()
    {
        _timeRemaining -= Time.deltaTime;
        _timerText.text = Mathf.CeilToInt(_timeRemaining).ToString(); // Отображаем только секунды

        if (_timeRemaining <= 0)
        {
            EndMiniGame(false); // Завершаем игру, если время вышло
        }
    }

    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0) && !_isProcessingClick) // ЛКМ и клик не обрабатывается
        {
            StartCoroutine(ProcessClick()); // Обрабатываем клик
        }
    }

    IEnumerator ProcessClick()
    {
        _isProcessingClick = true; // Блокируем обработку новых кликов

        // Включаем/выключаем лампочку под текущим диодом
        ToggleLampUnderCurrentDiode();

        // Меняем направление движения диодов
        _isClockwise = !_isClockwise;

        // Ждем небольшое время перед переключением диода
        yield return new WaitForSeconds(0.1f); // Задержка для визуальной обратной связи

        _isProcessingClick = false; // Разблокируем обработку кликов
    }

    private void ToggleLampUnderCurrentDiode()
    {
        // Получаем лампочку под текущим диодом
        Image lamp = lamps[_currentDiodeIndex];

        // Включаем/выключаем лампочку
        if (lamp.color == Color.gray)
        {
            lamp.color = Color.yellow; // Включаем лампочку
        }
        else
        {
            lamp.color = Color.gray; // Выключаем лампочку
        }

        // Проверяем, все ли лампочки зажжены
        if (CheckWinCondition())
        {
            EndMiniGame(true); // Завершаем игру победой
        }
    }

    IEnumerator ActivateDiodes()
    {
        while (_isGameActive)
        {
            // Выключаем все диоды
            foreach (var diode in diodes)
            {
                diode.color = Color.gray;
            }

            // Включаем текущий диод
            diodes[_currentDiodeIndex].color = Color.green;

            // Ждем перед следующим переключением
            yield return new WaitForSeconds(_diodeDelay);

            // Переходим к следующему диоду в зависимости от направления
            if (_isClockwise)
            {
                _currentDiodeIndex = (_currentDiodeIndex + 1) % diodes.Count; // По часовой стрелке
            }
            else
            {
                _currentDiodeIndex = (_currentDiodeIndex - 1 + diodes.Count) % diodes.Count; // Против часовой стрелки
            }
        }
    }

    private void ResetMiniGame()
    {
        foreach (var lamp in lamps)
        {
            lamp.color = Color.gray; // Выключаем все лампочки
        }
        foreach (var diode in diodes)
        {
            diode.color = Color.gray; // Выключаем все диоды
        }
    }

    private bool CheckWinCondition()
    {
        foreach (var lamp in lamps)
        {
            if (lamp.color == Color.gray)
            {
                return false; // Есть выключенные лампочки
            }
        }
        return true; // Все лампочки включены
    }
}
