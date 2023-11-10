using UnityEngine;

namespace CollectorBots
{
    public class Bot : MovementObject
    {
        [SerializeField] private Transform _handSlotTransform;

        private BotBase _botBase;
        private Resource _targetResource;
        private Flag _targetFlag;

        private bool _isFree = true;
        private bool _isTargetResourceReceived = false;

        private Resource _handSlot;

        public bool IsFree => _isFree;

        private void FixedUpdate()
        {
            if (_isFree)
                return;
            
            if (_targetFlag != null)
            {
                if (_botBase.IsSelected == false)
                    MoveTo(_targetFlag.transform.position);
            }
            else
            {
                if (_targetResource.gameObject.activeSelf == false || 
                    (_isTargetResourceReceived == false && _targetResource.transform.parent != _botBase.ResourcesContainer))
                {
                    _botBase.AbortResourceProcessing(_targetResource);

                    _targetResource = null;
                    _isFree = true;
                    return;
                }

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
            else if (other.TryGetComponent(out BotBase botBase) 
                && botBase == _botBase && _isTargetResourceReceived)
            {
                LoadResourceInBase(botBase);
            }
            else if (other.TryGetComponent(out Flag flag) && flag == _targetFlag)
            {
                CreateBase(flag);
            }
        }

        public void Initialise(BotBase botBase)
        {
            if (_botBase != null)
                throw new UnityException("The bot is already initialized.");

            _botBase = botBase;
        }

        public void SetTask(Resource resource)
        {
            _targetResource = resource;
            _isFree = false;
        }

        public void SetTask(Flag flag)
        {
            _targetFlag = flag;
            _isFree = false;
        }

        private void TakeResource(Resource resource)
        {
            _handSlot = resource;

            resource.transform.parent = transform;
            resource.transform.position = _handSlotTransform.position;

            _isTargetResourceReceived = true;
        }

        private void LoadResourceInBase(BotBase botBase)
        {
            _isFree = true;
            _isTargetResourceReceived = false;

            botBase.LoadResource(_handSlot);
            _handSlot = null;
        }

        private void CreateBase(Flag flag)
        {
            _botBase.DisconnectBot(this);

            _botBase = _botBase.CreateBase(flag.transform.position);

            _botBase.ConnectBot(this);

            _targetFlag = null;
            _isFree = true;
        }
    }
}