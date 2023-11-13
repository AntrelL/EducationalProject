using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots
{
    [RequireComponent(typeof(Scanner))]
    [RequireComponent(typeof(BotSpawner))]
    public class BotBase : MonoBehaviour
    {
        [SerializeField] private int _initialNumberOfBots;
        [SerializeField] private float _maxNumberOfBots;
        [SerializeField] private int _priceOfBot;
        [SerializeField] private int _priceOfBase;
        [SerializeField] private BotBase _TemplateOfBase;
        [SerializeField] private Transform _resourcesContainer;
        [SerializeField] private BotBaseStatesIndicator _baseStatesIndicator;
        [SerializeField] private Flag _flag;

        private List<Bot> _bots = new List<Bot>();
        private Queue<Resource> _standbyResources = new Queue<Resource>(); 
        private List<Resource> _processedResources = new List<Resource>();
        private BotSpawner _botSpawner;
        private Scanner _scanner;

        private int _numberOfResources;

        private bool _isFlagSet = false;
        private bool _isSelected = false;
        private bool _isBaseConstructionTaskStarted = false;
        private bool _isFirstBase = true;

        public event UnityAction<int> NumberOfResourcesChanged;

        public bool IsFlagGone => _flag == null;
        public bool IsSelected => _isSelected;
        public Transform ResourcesContainer => _resourcesContainer;

        private int NumberOfResources
        {
            get => _numberOfResources;
            set
            {
                _numberOfResources = value;
                NumberOfResourcesChanged?.Invoke(_numberOfResources);
            }
        }

        private void Awake()
        {
            _scanner = GetComponent<Scanner>();
            _botSpawner = GetComponent<BotSpawner>();
        }

        private void OnEnable()
        {
            _scanner.ScanDetector.ResourceDetected += OnResourceDetected;
        }

        private void OnDisable()
        {
            _scanner.ScanDetector.ResourceDetected -= OnResourceDetected;
        }

        private void Start()
        {
            if (_isFirstBase == false)
                return;

            _flag.gameObject.SetActive(false);
            UpdateStatesIndicator();

            NumberOfResources = 0;

            _botSpawner.CreateInitialBots(_initialNumberOfBots, this);
            _scanner.StartScanCycles();
        }

        public void Initialise(Transform botsContainer, Transform resourcesContainer, Flag flag)
        {
            _botSpawner.Initialize(botsContainer);

            _resourcesContainer = resourcesContainer;
            _flag = flag;

            _isFirstBase = false;

            UpdateStatesIndicator();
            _scanner.StartScanCycles();
        }

        public void UpdateTasks() => CreateMaxTasks();

        public void LoadResource(Resource resource)
        {
            resource.transform.parent = _resourcesContainer;
            resource.gameObject.SetActive(false);
            _processedResources.Remove(resource);

            NumberOfResources++;
        }

        public void SetFlag(Vector3 position)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = position;

            _isFlagSet = true;
        }

        public void Select()
        {
            _isSelected = true;
            UpdateStatesIndicator();
        }

        public void Unselect()
        {
            _isSelected = false;
            UpdateStatesIndicator();
        }

        public BotBase CreateBase(Vector3 position)
        {
            BotBase botBase = Instantiate(_TemplateOfBase, position, Quaternion.identity);

            botBase.Initialise(_botSpawner.BotsContainer, _resourcesContainer, _flag);

            _isFlagSet = false;
            _isBaseConstructionTaskStarted = false;

            _flag.gameObject.SetActive(false);
            _flag = null;

            NumberOfResources -= _priceOfBase;
            UpdateStatesIndicator();

            return botBase;
        }

        public void ConnectBot(Bot bot)
        {
            _bots.Add(bot);
        }

        public void DisconnectBot(Bot bot)
        {
            _bots.Remove(bot);
        }

        public void AbortResourceProcessing(Resource resource)
        {
            _processedResources.Remove(resource);
        }

        private bool CheckResourceAvailability(Resource resource)
        {
            return (resource.gameObject.activeSelf == false ||
                resource.transform.parent != _resourcesContainer) == false;
        }

        private void OnResourceDetected(Resource resource)
        {
            if (_processedResources.Contains(resource) ||
                _standbyResources.Contains(resource))
                return;

            _standbyResources.Enqueue(resource);
            CreateMaxTasks();
        }

        private void CreateMaxTasks()
        {
            TryDoTasksForBase();

            while (TryCreateTask()) { }
        }

        private bool TryCreateTask()
        {
            if (TryGetFreeBot(out Bot bot) == false)
                return false;

            if (TryGetStandbyResource(out Resource resource) == false)
                return false;

            if (CheckResourceAvailability(resource) == false)
                return true;

            _processedResources.Add(resource);
            bot.SetTask(resource);

            return true;
        }

        private bool TryGetFreeBot(out Bot freeBot)
        {
            foreach (Bot bot in _bots)
            {
                if (bot.IsFree)
                {
                    freeBot = bot;
                    return true;
                }
            }

            freeBot = null;
            return false;
        }

        private bool TryGetStandbyResource(out Resource resource)
        {
            if (_standbyResources.Count != 0)
            {
                resource = _standbyResources.Dequeue();
                return true;
            }

            resource = null;
            return false;
        }

        private void TryDoTasksForBase()
        {
            if ((_isFlagSet == false && NumberOfResources >= _priceOfBot) ||
                (_isFlagSet && _isBaseConstructionTaskStarted && NumberOfResources >= _priceOfBase + _priceOfBot))
            {
                if (_bots.Count == _maxNumberOfBots)
                    return;

                NumberOfResources -= _priceOfBot;
                _botSpawner.CreateBot(this);
            }
            else if (_isFlagSet && NumberOfResources >= _priceOfBase &&
                _isBaseConstructionTaskStarted == false)
            {
                if (TryGetFreeBot(out Bot bot) == false)
                    return;

                bot.SetTask(_flag);
                _isBaseConstructionTaskStarted = true;
            }
        }

        private void UpdateStatesIndicator()
        {
            _baseStatesIndicator.ChangeTheDisplayedState(_isFlagSet, IsFlagGone, _isSelected);
        }
    }
}