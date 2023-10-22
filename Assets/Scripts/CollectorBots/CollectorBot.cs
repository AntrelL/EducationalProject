using UnityEngine;

namespace CollectorBots
{
    public class CollectorBot : MovementObject
    {
        [SerializeField] private Transform _handSlotTransform;

        private CollectorBotBase _botBase;
        private Resource _targetResource;

        public bool _isFree = true;
        public bool _isTargetResourceReceived = false;

        private Resource _handSlot;

        private void FixedUpdate()
        {
            if (_isFree == false)
            {
                MoveTo(_isTargetResourceReceived ?
                    _botBase.transform.position : _targetResource.transform.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Resource resource) && resource == _targetResource)
            {
                TakeResource(resource);
            }
            else if (other.TryGetComponent(out CollectorBotBase collectorBotBase) 
                && collectorBotBase == _botBase && _isTargetResourceReceived)
            {
                LoadResourceInBase(collectorBotBase);
            }
        }

        public void Initialise(CollectorBotBase collectorBotBase)
        {
            if (_botBase != null)
                throw new UnityException("The bot is already initialized.");

            _botBase = collectorBotBase;
        }

        public void SetTask(Resource resource)
        {
            _targetResource = resource;
            _isFree = false;
        }

        private void TakeResource(Resource resource)
        {
            _handSlot = resource;

            resource.transform.parent = transform;
            resource.transform.position = _handSlotTransform.position;

            _isTargetResourceReceived = true;
        }

        private void LoadResourceInBase(CollectorBotBase collectorBotBase)
        {
            _isFree = true;
            _isTargetResourceReceived = false;

            collectorBotBase.LoadResource(_handSlot);
            _handSlot = null;
        }
    }
}