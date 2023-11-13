using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots.BotPrograms
{
    [RequireComponent(typeof(MovementObject))]
    public class Movement : Program
    {
        private MovementObject _movementObject;
        private Transform _target;

        private UnityAction<GameObject> _programFinished;

        private void Awake()
        {
            _movementObject = GetComponent<MovementObject>();
        }

        private void FixedUpdate()
        {
            if (IsRunning)
                _movementObject.MoveTo(_target.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsRunning && other.gameObject == _target.gameObject)
                Finish(other.gameObject);
        }

        public void Run(Transform target, UnityAction<GameObject> programFinished)
        {
            _target = target;
            _programFinished = programFinished;

            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void Finish(GameObject target)
        {
            IsRunning = false;
            _programFinished(target);
        }
    }
}