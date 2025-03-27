using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TrashObject : MonoBehaviour
{
    [SerializeField] private float _upTemperatueCount = 100f;

    [Header("Effector")]
    [SerializeField] private GameObject _paffEffect;
    [SerializeField] private GameObject _touchEffect;
    [SerializeField] private float _effectLifeTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reactor"))
        {
            ReactorController reactor = other.gameObject.GetComponent<ReactorController>();

            GameObject newEffect = Instantiate(_touchEffect, transform.position, Quaternion.identity);
            Destroy(newEffect, _effectLifeTime);

            StartCoroutine(DestroyAnimation(gameObject, reactor));
        }
    }

    IEnumerator DestroyAnimation(GameObject _gameObject, ReactorController reactor)
    {
        transform.DOScale(0f, 0.2f).From(transform.localScale).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(0.2f);
        Destroy(_gameObject);
        reactor.UpTemperature(_upTemperatueCount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (_paffEffect != null)
            {
                GameObject newEffect = Instantiate(_paffEffect, transform.position, Quaternion.identity);
                Destroy(newEffect, _effectLifeTime);
            }
        }
    }
}