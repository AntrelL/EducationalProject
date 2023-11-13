using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots.BotPrograms
{
    [RequireComponent(typeof(Movement))]
    public class ResourceDelivery : Program
    {
        [SerializeField] private Transform _handSlotTransform;

        private Movement _movement;
        private BotBase _botBase;
        private Resource _targetResource;
        private Resource _handSlot;

        private UnityAction _programFinished;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _handSlot = null;
        }

        private void Update()
        {
            if (IsRunning && IsTargetResourceFree() == false)
                Stop();
        }

        public void Run(Resource targetResource, BotBase botBase, UnityAction programFinished)
        {
            _targetResource = targetResource;
            _botBase = botBase;
            _programFinished = programFinished;

            IsRunning = true;
            _movement.Run(_targetResource.transform, OnMovementToResourceFinished);
        }

        private void OnMovementToResourceFinished(GameObject targetResource)
        {
            TakeResource(targetResource.GetComponent<Resource>());
            _movement.Run(_botBase.transform, OnMovementToBaseFinished);
        }

        private void OnMovementToBaseFinished(GameObject botBase)
        {
            LoadResourceInBase(botBase.GetComponent<BotBase>());

            IsRunning = false;
            _programFinished();
        }

        private void Stop()
        {
            _movement.Stop();
            IsRunning = false;
            _programFinished();
        }

        private void TakeResource(Resource resource)
        {
            _handSlot = resource;

            resource.transform.parent = transform;
            resource.transform.position = _handSlotTransform.position;
        }

        private void LoadResourceInBase(BotBase botBase)
        {
            botBase.LoadResource(_handSlot);
            _handSlot = null;
        }

        private bool IsTargetResourceFree()
        {
            return (_targetResource.gameObject.activeSelf == false ||
                (_handSlot == null && _targetResource.transform.parent != _botBase.ResourcesContainer)) == false;
        }
    }
}