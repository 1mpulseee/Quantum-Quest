using UnityEngine;

public class ReactorPlace : MonoBehaviour
{
    [SerializeField] private GameObject reactorPrefab;
    [SerializeField] private BuyPromptUI buyPromptUI;

    private ReactorEconomy _reactorEconomy;
    private ReactorState _reactorState;

    private void Start()
    {
        if (reactorPrefab == null)
        {
            Debug.LogError("������ �������� �� ��������!");
            return;
        }

        _reactorEconomy = reactorPrefab.GetComponent<ReactorEconomy>();
        if (_reactorEconomy == null)
            Debug.LogError("��������� ReactorEconomy �� ������ �� ������� ��������!");

        _reactorState = new ReactorState(this, buyPromptUI);
        _reactorState.Initialize();
    }

    private void Update()
    {
        if (_reactorState.IsPlayerInRange && Input.GetKeyDown(KeyCode.E) && !_reactorState.HasReactor)
            TryBuyReactor();
    }

    private void TryBuyReactor()
    {
        if (_reactorEconomy == null)
        {
            Debug.LogError("ReactorEconomy �� ���������������!");
            return;
        }

        if (_reactorEconomy.TryBuyReactor())
        {
            SpawnReactor();
            _reactorState.SetReactorExists(true);
        }
    }

    private void SpawnReactor()
    {
        if (reactorPrefab != null)
        {
            GameObject reactor = Instantiate(reactorPrefab, transform.position, transform.rotation);
            reactor.transform.SetParent(transform);

            ReactorController reactorController = reactor.GetComponent<ReactorController>();
            if (reactorController != null)
                reactorController.OnReactorDestroyed.AddListener(OnReactorDestroyed);

            _reactorState.Initialize();
        }
        else
        {
            Debug.LogError("������ �������� �� ��������!");
        }
    }

    private void OnReactorDestroyed()
    {
        _reactorState.OnReactorDestroyed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_reactorState.HasReactor)
            _reactorState.SetPlayerInRange(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _reactorState.SetPlayerInRange(false);
    }
}

/// <summary>
/// ������� ���������
/// </summary>
public class ReactorState
{
    private readonly ReactorPlace _reactorPlace;
    private readonly BuyPromptUI _buyPromptUI;

    public bool HasReactor { get; private set; }
    public bool IsPlayerInRange { get; private set; }

    public ReactorState(ReactorPlace reactorPlace, BuyPromptUI buyPromptUI)
    {
        _reactorPlace = reactorPlace;
        _buyPromptUI = buyPromptUI;
    }

    public void Initialize()
    {
        HasReactor = _reactorPlace.GetComponentInChildren<ReactorController>() != null;
        UpdateUI();
    }

    public void SetReactorExists(bool exists)
    {
        HasReactor = exists;
        UpdateUI();
    }

    public void SetPlayerInRange(bool inRange)
    {
        IsPlayerInRange = inRange;
        UpdateUI();
    }

    public void OnReactorDestroyed()
    {
        HasReactor = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_buyPromptUI != null)
        {
            _buyPromptUI.SetVisible(IsPlayerInRange && !HasReactor);
        }
    }
}