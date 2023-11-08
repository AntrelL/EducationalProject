using System.Collections;
using UnityEngine;

namespace FlappyTerminator
{
    [RequireComponent(typeof(EnemyMover))]
    public class Enemy : Entity
    {
        [SerializeField] private float _delayShot;

        private Coroutine _cyclicShooting;

        private void OnEnable()
        {
            _cyclicShooting = StartCoroutine(ShootCyclically(_delayShot));
        }

        private void OnDisable()
        {
            StopCoroutine(_cyclicShooting);
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator ShootCyclically(float delay)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

            while (true)
            {
                yield return waitForSeconds;
                Shoot(transform.right);
            }
        }
    }
}