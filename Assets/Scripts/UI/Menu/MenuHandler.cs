using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using DG.Tweening;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private string _gameSceneName;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _aboutPanel;

    private GameObject _currentPanel;

    private void Awake()
    {
        _currentPanel = _menuPanel;
    }

    public void StartGame()
    {
        SceneTransition.SwitchToScene(_gameSceneName);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    private void SwitchPanel(GameObject newPanel)
    {
        if(_currentPanel == newPanel) return;
        Sequence panelSequence = DOTween.Sequence();
        panelSequence
            .Append(_currentPanel.transform.DOScale(0f, 0.2f).From(0.8f).SetEase(Ease.InQuad))
            .Append(newPanel.transform.DOScale(0.8f, 0.2f).From(0).SetEase(Ease.InQuad));

        _currentPanel = newPanel;
    }
    public void ShowMenu()
    {
        SwitchPanel(_menuPanel);
    }

    public void ShowSettings()
    {
        SwitchPanel(_settingsPanel);
    }

    public void ShowAbout()
    {
        SwitchPanel(_aboutPanel);
    }

}
