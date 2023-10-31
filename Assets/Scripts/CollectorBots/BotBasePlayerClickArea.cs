using UnityEngine;

namespace CollectorBots
{
    public class BotBasePlayerClickArea : MonoBehaviour
    {
        [SerializeField] private CollectorBotBase _collectorBotBase;

        public CollectorBotBase CollectorBotBase => _collectorBotBase;
    }
}