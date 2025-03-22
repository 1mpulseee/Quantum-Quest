using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpaceDustObject : MonoBehaviour
{
    [SerializeField] private float _lowTemperatueCount = 100f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reactor"))
        {
            ReactorController reactor = other.gameObject.GetComponent<ReactorController>();
            StartCoroutine(DestroyAnimation(gameObject, reactor));
        }
    }

    IEnumerator DestroyAnimation(GameObject _gameObject, ReactorController reactor)
    {
        transform.DOScale(0f, 0.2f).From(transform.localScale).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(0.2f);
        Destroy(_gameObject);
        reactor.LowTemperature(_lowTemperatueCount);
    }
}