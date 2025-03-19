using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public float ThrowingForce = 10f; //сила броска предмета
    public Transform HoldPoint;
    private PickableItem _currentItem;
    public bool CanPickUp() => _currentItem == null;

    public void SetCurrentItem(PickableItem item)
    {
        _currentItem = item;
    }

    public void ClearCurrentItem()
    {
        _currentItem = null;
    }
}