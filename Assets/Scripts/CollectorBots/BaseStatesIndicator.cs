using UnityEngine;

namespace CollectorBots
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BaseStatesIndicator : MonoBehaviour
    {
        [SerializeField] private Material _flagNotSet;
        [SerializeField] private Material _flagSet;
        [SerializeField] private Material _flagIsGone;
        [SerializeField] private Material _baseSelected;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void ChangeTheDisplayedState(bool isFlagSet, bool isFlagGone, bool isBaseSelected)
        {
            Material material;

            if (isBaseSelected)
                material = _baseSelected;
            else if (isFlagGone)
                material = _flagIsGone;
            else if (isFlagSet)
                material = _flagSet;
            else
                material = _flagNotSet;

            _meshRenderer.material = material;
        }
    }
}