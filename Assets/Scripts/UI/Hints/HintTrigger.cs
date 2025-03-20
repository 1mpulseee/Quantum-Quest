using System;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [Header("Текст подсказки")]
    [SerializeField] private string _message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HintManager.DisplayHintEvent?.Invoke(_message);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HintManager.HideHintEvent?.Invoke();
        }
    }
}
