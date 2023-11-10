using UnityEngine;

namespace CollectorBots
{
    public class BotBasePlayerClickArea : MonoBehaviour
    {
        [SerializeField] private BotBase _collectorBotBase;

        public BotBase CollectorBotBase => _collectorBotBase;
    }
}