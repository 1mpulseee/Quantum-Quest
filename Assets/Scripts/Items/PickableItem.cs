using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isHeld;
    private Transform _playerTransform;
    private Transform _holdPoint;
    private float _throwForce;
    [SerializeField] private SphereCollider _hintTriggerZone;

    public bool IsHeld => _isHeld;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(Transform playerTransform, Transform holdPoint, float throwForce)
    {
        _playerTransform = playerTransform;
        _holdPoint = holdPoint;
        _throwForce = throwForce;
    }

    public void PickUp()
    {
        _hintTriggerZone.enabled = false;
        HintManager.HideHintEvent?.Invoke();
        _rigidbody.isKinematic = true;
        transform.position = _holdPoint.position;
        transform.parent = _holdPoint;
        _isHeld = true;
    }

    public void Drop()
    {
        _hintTriggerZone.enabled = true;
        _rigidbody.isKinematic = false;
        transform.parent = null;
        _isHeld = false;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _rigidbody.AddForce(_playerTransform.forward * _throwForce, ForceMode.Impulse);
    }
}