using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
