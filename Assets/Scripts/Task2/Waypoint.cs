using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 Position => transform.position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Navigator navigator))
            navigator.OnTouchingWithPoint(this);
    }
}
