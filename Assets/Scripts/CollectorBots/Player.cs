using UnityEngine;

namespace CollectorBots
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask _ignoredLayerMask;
        [SerializeField] private LayerMask _groundLayer;

        private BotBase _selectedBase;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_ignoredLayerMask))
                {
                    if (hit.collider.gameObject.TryGetComponent(out BotBasePlayerClickArea botBasePlayerClickArea))
                        OnClickOnBotBase(botBasePlayerClickArea);

                    if (1 << hit.collider.gameObject.layer == _groundLayer.value && _selectedBase != null)
                        SetFlag(hit.point);
                }
            }
        }

        private void OnClickOnBotBase(BotBasePlayerClickArea botBasePlayerClickArea)
        {
            BotBase botBase = botBasePlayerClickArea.CollectorBotBase;

            if (botBase.IsFlagGone)
                return;

            if (_selectedBase == null)
            {
                SelectBase(botBase);
            }
            else if (_selectedBase == botBase)
            {
                UnselectBase();
            }
            else
            {
                _selectedBase.Unselect();
                SelectBase(botBase);
            }
        }

        private void SetFlag(Vector3 position)
        {
            _selectedBase.SetFlag(position);
            UnselectBase();
        }

        private void SelectBase(BotBase botBase)
        {
            _selectedBase = botBase;
            _selectedBase.Select();
        }

        private void UnselectBase()
        {
            _selectedBase.Unselect();
            _selectedBase = null;
        }
    }
}