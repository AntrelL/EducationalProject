using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots
{
    [RequireComponent(typeof(Scanner))]
    public class CollectorBotBase : MonoBehaviour
    {
        [SerializeField] private int _initialNumberOfBots;
        [SerializeField] private CollectorBot _botTemplate;
        [SerializeField] private float _maxNumberOfBots;
        [SerializeField] private Transform _botsContainer;
        [SerializeField] private Transform _resourcesContainer;
        [SerializeField] private Transform _platform;
        [SerializeField] private float _platformRadius;
        [SerializeField] private BaseStatesIndicator _baseStatesIndicator;
        [SerializeField] private Flag _flag;
        [SerializeField] private CollectorBotBase _TemplateOfBase;
        [SerializeField] private int _priceOfBot;
        [SerializeField] private int _priceOfBase;

        private List<CollectorBot> _bots = new List<CollectorBot>();
        private Queue<Resource> _standbyResources = new Queue<Resource>(); 
        private List<Resource> _processedResources = new List<Resource>();
        private Scanner _scanner;

        private int _numberOfResources;
        private float _botSpawnDistance;
        private float _botSpawnHeight;

        private bool _isFlagSet = false;
        private bool _isSelected = false;
        private bool _isBaseConstructionTaskStarted = false;
        private bool _isFirstBase = true;

        public event UnityAction<int> NumberOfResourcesChanged;

        public bool IsFlagGone => _flag == null;
        public bool IsSelected => _isSelected;

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
            float botSpawnDistanceFactor = 0.8f;

            _botSpawnDistance = _platformRadius * _platform.localScale.x * botSpawnDistanceFactor;

            _botSpawnHeight = _botTemplate.GetComponent<CapsuleCollider>().height / 2f;

            CreateInitialBots(_initialNumberOfBots);
            _scanner.StartScanCycles();
        }

        public void Initialise(Transform botsContainer, Transform resourcesContainer, Flag flag, float botSpawnHeight)
        {
            _botsContainer = botsContainer;
            _resourcesContainer = resourcesContainer;
            _flag = flag;

            _botSpawnHeight = botSpawnHeight;
            _isFirstBase = false;

            UpdateStatesIndicator();
            _scanner.StartScanCycles();
        }

        public void LoadResource(Resource resource)
        {
            resource.transform.parent = _resourcesContainer;
            resource.gameObject.SetActive(false);
            _processedResources.Remove(resource);

            NumberOfResources++;

            CreateMaxTasks();
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

        public CollectorBotBase CreateBase(Vector3 position)
        {
            CollectorBotBase collectorBotBase = Instantiate(_TemplateOfBase, position, Quaternion.identity);

            collectorBotBase.Initialise(_botsContainer, _resourcesContainer, _flag, _botSpawnHeight);

            _isFlagSet = false;
            _isBaseConstructionTaskStarted = false;

            _flag.gameObject.SetActive(false);
            _flag = null;

            NumberOfResources -= _priceOfBase;

            UpdateStatesIndicator();

            return collectorBotBase;
        }

        public void ConnectBot(CollectorBot bot)
        {
            _bots.Add(bot);
        }

        public void DisconnectBot(CollectorBot bot)
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

        private void CreateInitialBots(int numberOfBots)
        {
            float angleOfOneTurn = 360f / numberOfBots;

            for (int i = 0; i < numberOfBots; i++)
            {
                Quaternion rotatedDirection = Quaternion.AngleAxis(angleOfOneTurn * (i + 1), transform.up);
                CreateBotAtDistanceFromCenter(rotatedDirection * transform.forward);
            }
        }

        private void CreateBotAtDistanceFromCenter(Vector3 spawnDirectionOnBase)
        {
            Vector3 spawnPosition = spawnDirectionOnBase * _botSpawnDistance;
            spawnPosition.y = _botSpawnHeight;

            CreateBot(spawnPosition);
        }

        private void CreateBot(Vector3 spawnPosition)
        {
            CollectorBot bot = Instantiate(_botTemplate, spawnPosition,
                Quaternion.identity, _botsContainer);

            bot.Initialise(this, _resourcesContainer);
            _bots.Add(bot);
        }

        private void CreateBot()
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = _botSpawnHeight;

            CreateBot(spawnPosition);
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
            if (TryGetFreeBot(out CollectorBot bot) == false)
                return false;

            if (TryGetStandbyResource(out Resource resource) == false)
                return false;

            if (CheckResourceAvailability(resource) == false)
                return true;

            _processedResources.Add(resource);
            bot.SetTask(resource);

            return true;
        }

        private bool TryGetFreeBot(out CollectorBot freeBot)
        {
            foreach (CollectorBot bot in _bots)
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
                CreateBot();
            }
            else if (_isFlagSet && NumberOfResources >= _priceOfBase &&
                _isBaseConstructionTaskStarted == false)
            {
                if (TryGetFreeBot(out CollectorBot bot) == false)
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