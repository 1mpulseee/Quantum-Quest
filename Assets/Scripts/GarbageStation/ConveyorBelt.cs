using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] float PhysicsSpeed = 2.5f;
    [SerializeField] float AnimSpeed = 25f;
    [SerializeField] Material mat;

    Rigidbody rb;
    private void OnValidate()
    {
        rb ??= GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    private void FixedUpdate()
    {
        mat.mainTextureOffset = new Vector2(0f,Time.time * AnimSpeed * Time.fixedDeltaTime * -1f % 1f);
        Vector3 pos = rb.position;
        rb.position -= transform.forward * PhysicsSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}