using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    [SerializeField] private float _volumeChangeSpeed = 0f;
    [SerializeField] [Range(0, 1)] private float _maxVolume = 0f;

    private float _minVolume = 0;
    private bool _active = false;

    private AudioSource _audioSource;
    private MotionSensor[] _motionSensors;

    private Coroutine _soundVolumeChanger = null;
    private bool _changingVolume = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _minVolume;

        _motionSensors = GetComponentsInChildren<MotionSensor>();

        foreach (MotionSensor motionSensor in _motionSensors)
        {
            motionSensor.DetectedMotion += OnDetectedMotion;
        }
    }

    private void OnDetectedMotion()
    {
        _active = !_active;

        if (_soundVolumeChanger != null && _changingVolume)
            StopCoroutine(_soundVolumeChanger);

        _soundVolumeChanger = StartCoroutine(SmoothlyChangeSoundVolume(_active ? _maxVolume : _minVolume));
    }

    private IEnumerator SmoothlyChangeSoundVolume(float targetVolume)
    {
        _changingVolume = true;

        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume,
                targetVolume, _volumeChangeSpeed * Time.deltaTime);

            yield return null;
        }

        _changingVolume = false;
    }
}
