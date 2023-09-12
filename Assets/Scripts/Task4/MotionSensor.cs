using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    public delegate void DetectionHandler();

    public event DetectionHandler DetectedMotion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MovementObject>(out _))
            DetectedMotion?.Invoke();
    }
}
