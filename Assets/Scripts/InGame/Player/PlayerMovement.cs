using System;
using Cysharp.Threading.Tasks;
using InGame.Logic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;

namespace InGame.Player
{
    [RequireComponent(typeof(PlayerLookAround), typeof(PlayerInput), typeof(PlayerState))]
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public partial class PlayerMovement : MonoBehaviour
    {
        private PlayerLookAround _lookComponent;
        private PlayerInput _inputComponent;
        private PlayerState _stateComponent;

        public CharacterController Cc { get; private set; }
        private void Awake()
        {
            _lookComponent = gameObject.GetOrAddComponent<PlayerLookAround>();
            _inputComponent = gameObject.GetOrAddComponent<PlayerInput>();
            _stateComponent = gameObject.GetOrAddComponent<PlayerState>();
            
            Cc = gameObject.GetOrAddComponent<CharacterController>();
        }

        private async void Start()
        {
            await UniTask.WaitUntil(() => _inputComponent != null);
            _inputComponent.AddValueListener("Move", MoveHandler);
        }
    }

    public partial class PlayerMovement
    {
        [SerializeField] private Transform body;
        public Transform Body => body;
        
        public Vector2 input;
        public Vector3 moveVec;
        
        private float _rotationVelocity;
    }

    public partial class PlayerMovement
    {
        private void Update()
        {
            Horizontal();
            Move();
        }

        private void Horizontal()
        {
            if (_stateComponent.InputLock) return;
            var i = new Vector3(input.x, 0, input.y).normalized;
            if (i.magnitude > 0)
            {
                var targetRot = Mathf.Atan2(i.x, i.z) * Mathf.Rad2Deg +
                                _lookComponent.CamArm.transform.eulerAngles.y;
                var rotation = Mathf.SmoothDampAngle(body.eulerAngles.y, targetRot, ref _rotationVelocity, 0.12f);
                body.rotation = Quaternion.Euler(0, rotation, 0);
                
                moveVec = Quaternion.Euler(0.0f, targetRot, 0.0f) * Vector3.forward;
            }
            else
            {
                moveVec = Vector3.zero;
            }

        }

        private void Move()
        {
            Cc.Move((moveVec.normalized * GameData.PlayerLogic.Speed + new Vector3(0, _stateComponent.Gravity)) *
                    Time.deltaTime);
        }
    }
    
    public partial class PlayerMovement
    {
        private void MoveHandler(InputAction.CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var kc = hit.gameObject.GetComponent<Key>();
            if (kc != null)
            {
                kc.Get();
                return;
            }

            var door = hit.gameObject.GetComponent<Door>();
            if (door != null)
            {
                Time.timeScale = 0;
                GameManager.Instance.End();
                return;
            }
        }
    }
}