using UnityEngine;
using CollectorBots.BotPrograms;

namespace CollectorBots
{
    [RequireComponent(typeof(Building))]
    [RequireComponent(typeof(ResourceDelivery))]
    public class Bot : MonoBehaviour
    {
        private BotBase _botBase;
        private Building _building;
        private ResourceDelivery _resourceDelivery;

        private Program _currentProgram;

        public bool IsFree => _currentProgram == null;

        private void Awake()
        {
            _building = GetComponent<Building>();
            _resourceDelivery = GetComponent<ResourceDelivery>();

            _currentProgram = null;
        }

        public void Init(BotBase botBase)
        {
            if (_botBase != null)
                throw new UnityException("The bot is already initialized.");

            _botBase = botBase;
        }

        public void SetTask(Resource resource)
        {
            _resourceDelivery.Run(resource, _botBase, OnResourceDeliveryFinished);
            _currentProgram = _resourceDelivery;
        }

        public void SetTask(Flag flag)
        {
            _building.Run(flag, _botBase, OnBuildingFinished);
            _currentProgram = _building;
        }

        private void OnResourceDeliveryFinished()
        {
            _currentProgram = null;
            _botBase.UpdateTasks();
        }

        private void OnBuildingFinished(BotBase newBotBase)
        {
            _botBase.DisconnectBot(this);
            _botBase = newBotBase;
            _botBase.ConnectBot(this);

            _currentProgram = null;
        }
    }
}