using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectorBots
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _maxScanDistance;
        [SerializeField] private float _areaSpeed;
        [SerializeField] private ScanDetector _detector;

        public ScanDetector ScanDetector => _detector;

        private void Start()
        {
            _detector.Deactivate();
        }

        public void StartScanCycles()
        {
            StartCoroutine(ScanAreaForResources());
        }

        private IEnumerator ScanAreaForResources()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);

            yield return new WaitForSeconds(_delay / 2f);

            while (true)
            {
                _detector.Scale = Vector3.one;
                _detector.Activate();

                while (_detector.Scale.x < _maxScanDistance)
                {
                    _detector.Scale += Vector3.one * _areaSpeed * Time.deltaTime;

                    yield return null;
                }

                _detector.Deactivate();
                yield return waitForSeconds;
            }
        }
    }
}