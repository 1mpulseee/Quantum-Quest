using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;

public class MiniGameController : MonoBehaviour
{
    public UnityEvent<bool> OnMiniGameEnded; // ������� ��������� ����-����

    public List<Image> lamps; 
    public List<Image> diodes;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _diodeDelay = 1f; // �������� ����� �������������� ������
    [SerializeField] private float _gameDuration = 30f; // ������������ ����-���� � ��������

    private List<ReactorController> _reactors; // ������ ���� ���������
    private ReactorController _reactor;
    private int _currentDiodeIndex; // ������ �������� ��������� �����
    private bool _isGameActive; // ������� �� ����-����
    private float _timeRemaining; // ���������� �����
    private bool _isProcessingClick; // ���� ��� ��������� �����
    private bool _isClockwise = true; // ����������� �������� ������ (�� ������� �������)

    private void Start()
    {
        // ������� ��� �������� � �����
        _reactors = new List<ReactorController>(FindObjectsByType<ReactorController>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        //_reactor = FindFirstObjectByType<ReactorController>();

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
        _isClockwise = true; // �������� � �������� �� ������� �������

        // ���������� ��������� �������� � ������
        ResetMiniGame();

        StartCoroutine(ActivateDiodes()); // ��������� ������������ ������
        Debug.Log("����-���� ��������!");
    }

    public void EndMiniGame(bool isWin)
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        StopAllCoroutines(); // ������������� ������������ ������

        if (isWin)
        {
            Debug.Log("����-���� ��������!");
            // ������� ������� � ���������� ������������
            ReactorController coldestReactor = GetColdestReactor();
            if (coldestReactor != null)
            {
                coldestReactor.temperature = 450f; // ������������� �����������
            }
        }
        else
        {
            Debug.Log("����-���� ���������!");
        }

        OnMiniGameEnded.Invoke(isWin); // ���������� � ���������� ����-����
        gameObject.SetActive(false); // �������� ����-����
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
        _timerText.text = Mathf.CeilToInt(_timeRemaining).ToString(); // ���������� ������ �������

        if (_timeRemaining <= 0)
        {
            EndMiniGame(false); // ��������� ����, ���� ����� �����
        }
    }

    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0) && !_isProcessingClick) // ��� � ���� �� ��������������
        {
            StartCoroutine(ProcessClick()); // ������������ ����
        }
    }

    IEnumerator ProcessClick()
    {
        _isProcessingClick = true; // ��������� ��������� ����� ������

        // ��������/��������� �������� ��� ������� ������
        ToggleLampUnderCurrentDiode();

        // ������ ����������� �������� ������
        _isClockwise = !_isClockwise;

        // ���� ��������� ����� ����� ������������� �����
        yield return new WaitForSeconds(0.1f); // �������� ��� ���������� �������� �����

        _isProcessingClick = false; // ������������ ��������� ������
    }

    private void ToggleLampUnderCurrentDiode()
    {
        // �������� �������� ��� ������� ������
        Image lamp = lamps[_currentDiodeIndex];

        // ��������/��������� ��������
        if (lamp.color == Color.gray)
        {
            lamp.color = Color.yellow; // �������� ��������
        }
        else
        {
            lamp.color = Color.gray; // ��������� ��������
        }

        // ���������, ��� �� �������� �������
        if (CheckWinCondition())
        {
            EndMiniGame(true); // ��������� ���� �������
        }
    }

    IEnumerator ActivateDiodes()
    {
        while (_isGameActive)
        {
            // ��������� ��� �����
            foreach (var diode in diodes)
            {
                diode.color = Color.gray;
            }

            // �������� ������� ����
            diodes[_currentDiodeIndex].color = Color.green;

            // ���� ����� ��������� �������������
            yield return new WaitForSeconds(_diodeDelay);

            // ��������� � ���������� ����� � ����������� �� �����������
            if (_isClockwise)
            {
                _currentDiodeIndex = (_currentDiodeIndex + 1) % diodes.Count; // �� ������� �������
            }
            else
            {
                _currentDiodeIndex = (_currentDiodeIndex - 1 + diodes.Count) % diodes.Count; // ������ ������� �������
            }
        }
    }

    private void ResetMiniGame()
    {
        foreach (var lamp in lamps)
        {
            lamp.color = Color.gray; // ��������� ��� ��������
        }
        foreach (var diode in diodes)
        {
            diode.color = Color.gray; // ��������� ��� �����
        }
    }

    private bool CheckWinCondition()
    {
        foreach (var lamp in lamps)
        {
            if (lamp.color == Color.gray)
            {
                return false; // ���� ����������� ��������
            }
        }
        return true; // ��� �������� ��������
    }
}
