using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    public delegate void DetectionHandler();

    public event DetectionHandler OnDetectedMotion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MovementObject>(out _))
            OnDetectedMotion?.Invoke();
    }
}
