using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ScanDetector : MonoBehaviour
    {
        private CapsuleCollider _capsuleCollider;

        public event UnityAction<Resource> ResourceDetected;

        public Vector3 Scale
        {
            get => transform.localScale; 
            set => transform.localScale = value;
        }

        private void Start()
        {
            _capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Resource resource) == false)
                return;
            
            Vector3 resourcePosition = resource.transform.position;
            resourcePosition.y = transform.position.y;

            float distance = Vector3.Distance(resourcePosition, transform.position);

            if (distance >= _capsuleCollider.radius * transform.localScale.x)
                ResourceDetected?.Invoke(resource);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}