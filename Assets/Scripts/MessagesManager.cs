using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class MessagesManager : MonoBehaviour
{
    private const string RESOURCES_PATH = "Messages/";
    private const int ANIMATION_ID = 1;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField] private GameObject _skipHint;
    [SerializeField] private GameObject _messageContainer;


    float _delayBetweenHalves = .05f;
    float _delayBetweenCharacters = .03f;
    float _smoothTime = .1f;

    List<float> _leftAlphas = new();
    List<float> _rightAlphas = new();
    IEnumerator _animationCoroutine;
    MessageSettings _currentSettings;
    bool _isAnimating;

    public static MessagesManager Instance { get; private set; }

    private void Awake()
    {
        InitializeSingleton();
        ResetUIState();
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void ResetUIState()
    {
        _skipHint.SetActive(false);
        _messageContainer.SetActive(false);
    }

    private void Update()
    {
        if (_isAnimating)
            SwitchColor();
    }

    public void StartMessages(string messageName)
    {
        if (_isAnimating)
        {
            Debug.LogWarning("Message sequence is already running!");
            return;
        }

        _currentSettings = Resources.Load<MessageSettings>($"{RESOURCES_PATH}{messageName}");

        if (_currentSettings != null)
        {
            StartCoroutine(MessageLogic());
        }
        else
        {
            Debug.LogError($"MessageSettings with name {messageName} not found in Resources/Messages");
        }
    }

    IEnumerator MessageLogic()
    {
        _messageContainer.SetActive(true);
        if (_currentSettings.IsPaused)
            Time.timeScale = 0f;

        for (int i = 0; i < _currentSettings.Messages.Length; i++)
        {
            _textComponent.text = _currentSettings.Messages[i];
            StartTextAnim();

            yield return new WaitUntil(() => !_isAnimating || (Input.anyKeyDown && _currentSettings.IsPaused));
            yield return null;
            Visible(true);
            _isAnimating = false;
            if (!_currentSettings.AutoSkip)
                _skipHint.SetActive(true);
            yield return new WaitUntil(() => Input.anyKeyDown || _currentSettings.AutoSkip);
            if (_currentSettings.AutoSkip)
                yield return new WaitForSecondsRealtime(_currentSettings.SkipDelay);
            else
                _skipHint.SetActive(false);

            yield return null;
        }

        _messageContainer.SetActive(false);
        if (_currentSettings.IsPaused)
            Time.timeScale = 1f;
    }

    IEnumerator Smooth(int i)
    {
        if (i >= _leftAlphas.Count)
        {
            _isAnimating = false;
            _animationCoroutine = null;
            yield break;
        }

        DOTween.To(
            () => _leftAlphas[i],
            x => _leftAlphas[i] = x,
            255,
            _smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1)
            .SetUpdate(true);
        yield return new WaitForSecondsRealtime(_delayBetweenHalves);

        DOTween.To(
            () => _rightAlphas[i],
            x => _rightAlphas[i] = x,
            255,
            _smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1)
            .SetUpdate(true);
        yield return new WaitForSecondsRealtime(_delayBetweenCharacters);

        _animationCoroutine = Smooth(i + 1);
        StartCoroutine(_animationCoroutine);
    }

    void StartTextAnim()
    {
        _textComponent.ForceMeshUpdate();
        _leftAlphas = new float[_textComponent.text.Length].ToList();
        _rightAlphas = new float[_textComponent.text.Length].ToList();

        _delayBetweenHalves = _currentSettings.DelayBetweenHalves;
        _delayBetweenCharacters = _currentSettings.DelayBetweenCharacters;
        _smoothTime = _currentSettings.SmoothTime;

        Visible(false);
        _isAnimating = true;
        _animationCoroutine = Smooth(0);
        StartCoroutine(_animationCoroutine);
    }
    void Visible(bool visible)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        DOTween.Kill(1);

        for (int i = 0; i < _leftAlphas.Count; i++)
        {
            _leftAlphas[i] = visible ? 255 : 0;
            _rightAlphas[i] = visible ? 255 : 0;
        }
        SwitchColor();
    }

    void SwitchColor()
    {
        for (int i = 0; i < _leftAlphas.Count; i++)
        {
            if (_textComponent.textInfo.characterInfo[i].character != '\n' &&
                _textComponent.textInfo.characterInfo[i].character != ' ')
            {
                int meshIndex = _textComponent.textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = _textComponent.textInfo.characterInfo[i].vertexIndex;

                Color32[] vertexColors = _textComponent.textInfo.meshInfo[meshIndex].colors32;

                vertexColors[vertexIndex + 0].a = (byte)_leftAlphas[i];
                vertexColors[vertexIndex + 1].a = (byte)_leftAlphas[i];
                vertexColors[vertexIndex + 2].a = (byte)_rightAlphas[i];
                vertexColors[vertexIndex + 3].a = (byte)_rightAlphas[i];
            }
        }
        _textComponent.UpdateVertexData();
    }

    #region Singleton Lifecycle
#if UNITY_EDITOR
    [UnityEditor.InitializeOnEnterPlayMode]
    static void ResetOnEnterPlayMode() => Instance = null;
#else
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
static void ResetStaticVariables() => Instance = null;
#endif
    #endregion
}