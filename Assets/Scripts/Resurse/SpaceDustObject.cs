using UnityEngine;

public class SpaceDustObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Reactor"))
        {
            ReactorController reactor = collision.gameObject.GetComponent<ReactorController>();
            reactor.LowTemperature(100);
            Destroy(gameObject);
        }
    }
}
