using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementObject : MonoBehaviour, IWaitingObject
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody _rigidbody;
    private bool _waiting = false;

    public float MovementSpeed
    {
        get => _movementSpeed;
        set
        {
            _movementSpeed = ClampInPositiveRange(value);
        }
    }

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set
        {
            _rotationSpeed = ClampInPositiveRange(value);
        }
    }

    protected bool Waiting
    {
        get => _waiting;
    }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnValidate()
    {
        MovementSpeed = _movementSpeed;
        RotationSpeed = _rotationSpeed;
    }

    public IEnumerator Wait(float time)
    {
        _waiting = true;
        yield return new WaitForSeconds(time);
        _waiting = false;
    }

    protected void MoveTo(Vector3 targetPosition, bool useRotation = true, bool yMovement = false)
    {
        Vector3 movementDirection = (targetPosition - transform.position).normalized;

        if (yMovement == false)
            movementDirection.y = 0;

        Vector3 velocity;

        if (useRotation)
        {
            Vector3 rotationDirection = Vector3.RotateTowards(transform.forward,
                movementDirection, RotationSpeed, 0f);
            _rigidbody.rotation = Quaternion.LookRotation(rotationDirection);

            velocity = transform.forward;
        }
        else
        {
            velocity = movementDirection;
        }

        velocity *= MovementSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(transform.position + velocity);
    }

    private float ClampInPositiveRange(float value) => Mathf.Max(value, 0f);
}
