using UnityEngine;

public interface IResourceFactory
{
    GameObject CreateResource(Vector3 position, Quaternion rotation);
}
