using System;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static Action<GameObject> EnableOutlineEvent;
    public static Action<GameObject> DisableOutlineEvent;
    
    private void OnEnable()
    {
        EnableOutlineEvent += EnableOutline;
        DisableOutlineEvent += DisableOutline;
    }

    private void OnDisable()
    {
        EnableOutlineEvent -= EnableOutline;
        DisableOutlineEvent -= DisableOutline;
    }

    
    private void EnableOutline(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        Transform modelTransform = parent.Find("model");
        modelTransform.gameObject.layer = LayerMask.NameToLayer("Outline");
    }

    private void DisableOutline(GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        Transform modelTransform = parent.Find("model");
        modelTransform.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}