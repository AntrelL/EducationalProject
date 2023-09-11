using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    [SerializeField] private float _volumeChangeSpeed;
    [SerializeField] [Range(0, 1)] private float _maxVolume;

    private float _minVolume = 0;
    private float _currentVolume;

    private bool _active = false;

    private AudioSource _audioSource;
    private MotionSensor[] _motionSensors;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentVolume = _minVolume;

        _motionSensors = GetComponentsInChildren<MotionSensor>();

        foreach (MotionSensor motionSensor in _motionSensors)
        {
            motionSensor.OnDetectedMotion += OnDetectedMotion;
        }
    }

    private void Update()
    {
        _currentVolume = Mathf.MoveTowards(_currentVolume,
            _active ? _maxVolume : _minVolume, _volumeChangeSpeed * Time.deltaTime);

        _audioSource.volume = _currentVolume;
    }

    private void OnDetectedMotion()
    {
        _active = !_active;
    }
}
