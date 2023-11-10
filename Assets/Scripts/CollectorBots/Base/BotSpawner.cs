using UnityEngine;

namespace CollectorBots
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField] private Bot _botTemplate;
        [SerializeField] private Transform _botsContainer;
        [SerializeField] private Transform _platform;
        [SerializeField] private float _platformRadius;

        private float _botSpawnDistance;
        private float _botSpawnHeight;

        public Transform BotsContainer => _botsContainer;

        private void Start()
        {
            float botCenterPositionY = _botTemplate.GetComponent<CapsuleCollider>().height / 2f;
            _botSpawnHeight = botCenterPositionY;

            float botSpawnDistanceFactor = 0.8f;
            _botSpawnDistance = _platformRadius * _platform.localScale.x * botSpawnDistanceFactor;
        }

        public void Initialize(Transform botsContainer)
        {
            _botsContainer = botsContainer;
        }

        public void CreateInitialBots(int numberOfBots, BotBase botBase)
        {
            float numberOfDegreesInCircle = 360f;
            float angleOfOneTurn = numberOfDegreesInCircle / numberOfBots;

            for (int i = 0; i < numberOfBots; i++)
            {
                Quaternion rotatedDirection = Quaternion.AngleAxis(angleOfOneTurn * (i + 1), transform.up);
                CreateBotAtDistanceFromCenter(rotatedDirection * transform.forward, botBase);
            }
        }

        public void CreateBot(BotBase botBase)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = _botSpawnHeight;

            CreateBot(spawnPosition, botBase);
        }

        private void CreateBotAtDistanceFromCenter(Vector3 spawnDirectionOnBase, BotBase botBase)
        {
            Vector3 spawnPosition = spawnDirectionOnBase * _botSpawnDistance;
            spawnPosition.y = _botSpawnHeight;

            CreateBot(spawnPosition, botBase);
        }

        private void CreateBot(Vector3 spawnPosition, BotBase botBase)
        {
            Bot bot = Instantiate(_botTemplate, spawnPosition,
                Quaternion.identity, _botsContainer);

            bot.Initialise(botBase);
            botBase.ConnectBot(bot);
        }
    }
}