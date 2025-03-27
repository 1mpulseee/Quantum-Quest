using UnityEngine;
using System.Collections;

public class ReactorEffects : MonoBehaviour
{
    [SerializeField] private GameObject _effectPrefab;
    [SerializeField] private float _effectLifeTime;
    [SerializeField] private float _minDelay = 1f;
    [SerializeField] private float _maxDelay = 10f;

    private void Start()
    {
        StartCoroutine(SpawnEffectWithRandomDelay());
    }

    private IEnumerator SpawnEffectWithRandomDelay()
    {
        while (true)
        {
            float delay = Random.Range(_minDelay, _maxDelay);
            yield return new WaitForSeconds(delay);

            GameObject newEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity);
            Destroy(newEffect, _effectLifeTime); 
        }
    }
}
