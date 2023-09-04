using UnityEngine;

[RequireComponent(typeof(Navigator))]
public class EnemyTarget : MovementObject
{
    private Navigator _navigator;

    private void Start()
    {
        _navigator = GetComponent<Navigator>();
    }

    private void FixedUpdate()
    {
        MoveTo(_navigator.CurrentTargetPoint.Position, false, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out _))
            Destroy(other.gameObject);
    }
}