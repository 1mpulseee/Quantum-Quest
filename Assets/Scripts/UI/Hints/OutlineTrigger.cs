using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OutlineManager.EnableOutlineEvent?.Invoke(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OutlineManager.DisableOutlineEvent?.Invoke(gameObject);
        }
    }
}