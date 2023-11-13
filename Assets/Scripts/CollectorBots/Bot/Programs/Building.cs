using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots.BotPrograms
{
    [RequireComponent(typeof(Movement))]
    public class Building : Program
    {
        private Movement _movement;
        private BotBase _botBase;
        private Flag _targetFlag;

        private UnityAction<BotBase> _programFinished;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        public void Run(Flag flag, BotBase botBase, UnityAction<BotBase> programFinished)
        {
            _targetFlag = flag;
            _botBase = botBase;
            _programFinished = programFinished;

            IsRunning = true;
            _movement.Run(_targetFlag.transform, OnMovementToFlagFinished);
        }

        private void OnMovementToFlagFinished(GameObject flag)
        {
            BotBase newBotBase = _botBase.CreateBase(flag.transform.position);

            IsRunning = false;
            _programFinished(newBotBase);
        }
    }
}