using System;
using UnityEngine;
using DG.Tweening;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private string _menuSceneName;
    [SerializeField] private string _gameSceneName;
    
    private bool _isPaused;

    /*private void Awake()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }*/

    private void Update()
    {
        Pause();
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            PauseMenuPopup(_isPaused);
            Time.timeScale = 0f;
            _isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            PauseMenuPopup(_isPaused);
            Time.timeScale = 1f;
            _isPaused = false;
        }
    }

    private void PauseMenuPopup(bool isPaused)
    {
        if(!isPaused)
            _pauseMenu.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBounce).SetUpdate(true);
        else
            _pauseMenu.transform.DOScale(0, 0.2f).From(1).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneTransition.SwitchToScene(_gameSceneName);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneTransition.SwitchToScene(_menuSceneName);
    }
}
