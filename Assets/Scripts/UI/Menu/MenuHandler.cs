using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private string _gameSceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
