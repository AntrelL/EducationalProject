using System.Collections;
using UnityEngine;

public class InstantBulletShooting : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Rigidbody _bulletPattern;
    [SerializeField] private float _delayBetweenShots;
    [SerializeField] private Transform _target;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        bool isShooting = enabled;
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delayBetweenShots);

        while (isShooting)
        {
            Vector3 shotDirection = (_target.position - transform.position).normalized;
            Rigidbody bullet = Instantiate(_bulletPattern, transform.position + shotDirection, Quaternion.identity);

            bullet.transform.up = shotDirection;
            bullet.velocity = shotDirection * _bulletSpeed;

            yield return waitForSeconds;
        }
    }
}