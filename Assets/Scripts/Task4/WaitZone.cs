using UnityEngine;

public class WaitZone : MonoBehaviour
{
    [SerializeField] [Tooltip("In seconds")] private float _waitingTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IWaitingObject waitingObject))
            StartCoroutine(waitingObject.Wait(_waitingTime));
    }
}
