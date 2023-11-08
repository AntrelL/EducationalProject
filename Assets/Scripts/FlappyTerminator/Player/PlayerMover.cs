using UnityEngine;

namespace FlappyTerminator
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _tapForce;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _rotationEnergyUpStep;
        [SerializeField] private float _rotationEnergyDownStep;
        [SerializeField] private float _maxRotationEnergy;
        [SerializeField] private float _maxRotationZ;
        [SerializeField] private float _minRotationZ;

        private Rigidbody2D _rigidbody;
        private Vector3 _startPosition;

        private float _rotationEnergy = 0;
        private (Quaternion Max, Quaternion Min) _rotationLimit;

        private float RotationEnergy
        {
            get => _rotationEnergy;
            set
            {
                _rotationEnergy = Mathf.Clamp(value, -1, _maxRotationEnergy);
            }
        }

        private void Start()
        {
            _startPosition = transform.position;

            _rotationLimit.Max = Quaternion.Euler(0, 0, _maxRotationZ);
            _rotationLimit.Min = Quaternion.Euler(0, 0, _minRotationZ);

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = Vector2.zero;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(Vector2.up * _tapForce, ForceMode2D.Force);

                RotationEnergy += _rotationEnergyUpStep;
            }
            else
            {
                RotationEnergy -= _rotationEnergyDownStep;
            }

            Rotate(RotationEnergy, _rotationSpeed);
        }

        public void ResetValues()
        {
            transform.position = _startPosition;
            transform.rotation = Quaternion.identity;
            _rigidbody.velocity = Vector2.zero;

            RotationEnergy = 0;
        }

        private void Rotate(float rotationEnergy, float rotationSpeed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                 rotationEnergy >= 0 ? _rotationLimit.Max : _rotationLimit.Min,
                 rotationSpeed * Time.deltaTime);
        }
    }
}