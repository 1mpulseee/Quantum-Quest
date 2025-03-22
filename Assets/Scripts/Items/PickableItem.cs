using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isHeld;
    private Transform _playerTransform;
    private Transform _holdPoint;
    private float _throwForce;
    [SerializeField] private SphereCollider _hintTriggerZone;
    private GameObject _gameObject;
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
        transform.DOScale(0f, 0.2f).From(transform.localScale).SetEase(Ease.Linear);
        
        _hintTriggerZone.enabled = false;
        
        _rigidbody.isKinematic = true;
        transform.position = _holdPoint.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.parent = _holdPoint;
        
        transform.DOScale(transform.localScale, 0.2f).From(0f).SetEase(Ease.OutBounce);
        _isHeld = true;
        HintManager.HideHintEvent?.Invoke();
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