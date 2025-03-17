using System;
using UnityEngine;

public class GarbageChecker : MonoBehaviour
{
    public int GarbageCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Garbage"))
        {
            GarbageCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Garbage"))
        {
            GarbageCount -= 1;
        }
    }
}