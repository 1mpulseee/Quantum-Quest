using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private IInputReader _inputReader;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputReader = new KeyboardInputReader();
    }

    private void Update()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        if (_inputReader.GetMovementInput().magnitude > 0.1f)
            _animator.SetBool("IsRunning", true);
        else
            _animator.SetBool("IsRunning", false);
    }
}