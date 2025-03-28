using UnityEngine;

public class ButtonsSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _buttonSound;
    
    public void OnClick()
    {
        _buttonSound.Play();    
    }
}
