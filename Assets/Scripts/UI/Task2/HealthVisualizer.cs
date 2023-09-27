using UnityEngine;

namespace UITask2
{
    public abstract class HealthVisualizer : MonoBehaviour
    {
        public virtual void UpdateHealthValue(float value) { }
    }
}