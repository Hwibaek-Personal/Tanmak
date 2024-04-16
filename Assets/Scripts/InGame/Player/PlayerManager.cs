using Cysharp.Threading.Tasks;
using UnityEngine;
using Utility;

namespace InGame.Player
{
    [RequireComponent(
        typeof(PlayerEventHandler),
        typeof(PlayerMovement),
        typeof(PlayerState))
    ]
    [DisallowMultipleComponent]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerEventHandler EventHandler { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerLookAround LookAround { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerState State { get; private set; }
        
        private async void Awake()
        {
            EventHandler = gameObject.GetOrAddComponent<PlayerEventHandler>();
            Input = gameObject.GetOrAddComponent<PlayerInput>();
            LookAround = gameObject.GetOrAddComponent<PlayerLookAround>();
            Movement = gameObject.GetOrAddComponent<PlayerMovement>();
            State = gameObject.GetOrAddComponent<PlayerState>();
            await UniTask.WaitUntil(() => GameManager.Instance);
            GameManager.Instance.player.I = this;
        }
    }
}