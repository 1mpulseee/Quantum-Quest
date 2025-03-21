using UnityEngine;

public class TrashObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Reactor"))
        {
            ReactorController reactor = collision.gameObject.GetComponent<ReactorController>();
            reactor.UpTemperature(100);
            Destroy(gameObject);
        }
    }
}
