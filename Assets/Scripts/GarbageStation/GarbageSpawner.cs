using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GarbageSpawner : MonoBehaviour
{
    [SerializeField] private float _resourceSpawnDelay = 5f;
    [SerializeField] private Transform _resourceSpawnPoint;
    [SerializeField] private GarbageChecker _garbageChecker;
    [SerializeField] private GameObject[] _resource;

    private float _timer;
    private IResourceFactory _factory;

    private void Start()
    {
        _timer = _resourceSpawnDelay;
        _factory = new RandomResourceFactory(_resource);
    }

    void Update()
    {
        if (_garbageChecker.GarbageCount == 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Spawn();
                _timer = _resourceSpawnDelay;
            }
        }
    }

    private void Spawn()
    {
        _factory.CreateResource(_resourceSpawnPoint.position, _resourceSpawnPoint.rotation);
    }
}