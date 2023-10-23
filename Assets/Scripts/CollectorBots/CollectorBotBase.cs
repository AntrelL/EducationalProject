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
        [SerializeField] private Transform _botsContainer;
        [SerializeField] CapsuleCollider _mainCollider;
        [SerializeField] Transform _ResourcesContainer;

        private List<CollectorBot> _bots = new List<CollectorBot>();
        private Queue<Resource> _standbyResources = new Queue<Resource>(); 
        private List<Resource> _processedResources = new List<Resource>();
        private Scanner _scanner;

        private int _numberOfResources;
        private float _botSpawnDistance;
        private float _botSpawnHeight;

        public UnityAction<int> NumberOfResourcesChanged;

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
            NumberOfResources = 0;
            float botSpawnDistanceFactor = 0.8f;

            _botSpawnDistance = _mainCollider.radius *
                _mainCollider.transform.localScale.x * botSpawnDistanceFactor;

            _botSpawnHeight = _botTemplate.GetComponent<CapsuleCollider>().height / 2f;

            CreateInitialBots(_initialNumberOfBots);
            _scanner.StartScanCycles();
        }

        public void LoadResource(Resource resource)
        {
            resource.transform.parent = _ResourcesContainer;
            resource.gameObject.SetActive(false);
            _processedResources.Remove(resource);

            NumberOfResources++;

            CreateMaxTasks();
        }

        private void CreateInitialBots(int numberOfBots)
        {
            float angleOfOneTurn = 360f / numberOfBots;

            for (int i = 0; i < numberOfBots; i++)
            {
                Quaternion rotatedDirection = Quaternion.AngleAxis(angleOfOneTurn * (i + 1), transform.up);
                CreateBot(rotatedDirection * transform.forward);
            }
        }

        private void CreateBot(Vector3 spawnDirectionOnBase)
        {
            Vector3 spawnPosition = spawnDirectionOnBase * _botSpawnDistance;
            spawnPosition.y = _botSpawnHeight;

            CollectorBot bot = Instantiate(_botTemplate, spawnPosition,
                Quaternion.identity, _botsContainer);

            bot.Initialise(this);
            _bots.Add(bot);
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
            while (TryCreateTask()) { }
        }

        private bool TryCreateTask()
        {
            if (TryGetFreeBot(out CollectorBot bot) == false)
                return false;

            if (TryGetStandbyResource(out Resource resource) == false)
                return false;

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
    }
}