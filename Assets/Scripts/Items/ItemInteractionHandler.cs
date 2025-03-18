using UnityEngine;

public class ItemInteractionHandler : MonoBehaviour
{
    private PickableItem _pickableItem;
    private PickUpManager _pickUpManager;
    private bool _isInRange;

    private void Awake()
    {
        _pickableItem = GetComponent<PickableItem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _pickUpManager = other.GetComponent<PickUpManager>();

            if (_pickUpManager != null && _pickableItem != null)
            {
                _pickableItem.Initialize(other.transform, _pickUpManager.HoldPoint, _pickUpManager.ThrowingForce);
                _isInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInRange = false;
        }
    }

    private void Update()
    {
        if (_isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (_pickableItem.IsHeld)
            {
                _pickableItem.Drop();
            }
            else
            {
                _pickableItem.PickUp();
            }
        }
    }
}