using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    private IInputReader _inputReader;

    private void Start()
    {
        _inputReader = new KeyboardInputReader();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movementDirection = _inputReader.GetMovementInput();

        if (movementDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed);
        }

        transform.position += movementDirection * _moveSpeed * Time.deltaTime;
    }
}