using System;
using System.Collections;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(LineRenderer))]
    public class VampirismSkill : MonoBehaviour
    {
        private const int NumberOfPointsToDrawLine = 2;

        [SerializeField] private float _duration;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _radius;
        [SerializeField] private float _healthTransferTimeInterval;
        [SerializeField, Min(0)] private int _healthTransferValue;
        [SerializeField] private Entity _receivingEntity;
        [SerializeField] private LayerMask _targetsLayer;

        private SkillState _state;
        private LineRenderer _lineRenderer;
        private float _healthTransferTimer = 0;

        private enum SkillState
        {
            Ready,
            Activated,
            OnCooldown
        }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();   
        }

        private void Update()
        {
            if (_state != SkillState.Activated)
                return;

            Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _targetsLayer);
            Collider2D targetCollider = GetClosestCollider(targetColliders);

            if (targetCollider != null && targetCollider.TryGetComponent(out Entity target))
            {
                EnableLineDrawing();

                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, target.transform.position);

                if (_healthTransferTimer >= _healthTransferTimeInterval)
                {
                    TransferHealth(_receivingEntity, target);
                    _healthTransferTimer = 0;
                }
            }
            else
            {
                DisableLineDrawing();
            }

            _healthTransferTimer += Time.deltaTime;
        }

        public void TryActivate()
        {
            if (_state != SkillState.Ready)
                return;

            _state = SkillState.Activated;
            StartCoroutine(StopExecution(_duration));
        }  

        private IEnumerator StopExecution(float delay)
        {
            yield return new WaitForSeconds(delay);

            DisableLineDrawing();
            StartCoroutine(WaitCooldown());
        }

        private IEnumerator WaitCooldown()
        {
            _state = SkillState.OnCooldown;
            yield return new WaitForSeconds(_cooldown);

            _state = SkillState.Ready;
        }

        private Collider2D GetClosestCollider(Collider2D[] colliders)
        {
            Collider2D target = null;
            float minDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                float distance = (collider.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = collider;
                }
            }

            return target;
        }

        private void TransferHealth(Entity receivingEntity, Entity targetEntity)
        {
            int takenHealth = Math.Min(_healthTransferValue, targetEntity.Health);

            targetEntity.ApplyDamage(takenHealth);
            receivingEntity.ApplyHeal(takenHealth);
        }

        private void EnableLineDrawing() => _lineRenderer.positionCount = NumberOfPointsToDrawLine;

        private void DisableLineDrawing() => _lineRenderer.positionCount = 0;
    }
}