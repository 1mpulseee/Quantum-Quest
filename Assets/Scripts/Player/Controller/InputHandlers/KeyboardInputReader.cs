using UnityEngine;

public class KeyboardInputReader : IInputReader
{
    public Vector3 GetMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        return new Vector3(verticalInput, 0, -horizontalInput);
    }
}
