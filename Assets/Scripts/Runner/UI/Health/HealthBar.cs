using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private HealthCell _healthCellTemplate;
        [SerializeField] private Transform _healthCellContainer;

        private List<HealthCell> _healthCells = new List<HealthCell>();

        private void OnEnable()
        {
            _player.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _player.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int value)
        {
            if (_healthCells.Count < value)
            {
                int createHealthCell = value - _healthCells.Count;

                for (int i = 0; i < createHealthCell; i++)
                    CreateHealthCell();
            }
            else if (_healthCells.Count > value)
            {
                int deleteHealthCell = _healthCells.Count - value;

                for (int i = 0; i < deleteHealthCell; i++)
                    DestroyHealthCell(_healthCells[_healthCells.Count - 1]);
            }
        }

        private void CreateHealthCell()
        {
            HealthCell newHealthCell = Instantiate(_healthCellTemplate, _healthCellContainer);
            _healthCells.Add(newHealthCell);
        }

        private void DestroyHealthCell(HealthCell healthCell)
        {
            _healthCells.Remove(healthCell);
            healthCell.Destroy();
        }
    }
}