using System.Collections;
using UnityEngine;

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
            WaitForSeconds delay = new WaitForSeconds(_delay);
            float firstDelayFactor = 0.5f;

            yield return new WaitForSeconds(_delay * firstDelayFactor);

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
                yield return delay;
            }
        }
    }
}