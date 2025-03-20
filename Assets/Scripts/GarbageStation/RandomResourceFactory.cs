using UnityEngine;

public class RandomResourceFactory : IResourceFactory
{
    private readonly GameObject[] _resource;

    public RandomResourceFactory(GameObject[] resources)
    {
        _resource = resources;
    }

    public GameObject CreateResource(Vector3 position, Quaternion rotation)
    {
        if (_resource.Length == 0)
            return null;
        return Object.Instantiate(_resource[Random.Range(0, _resource.Length)], position, rotation);
    }
}