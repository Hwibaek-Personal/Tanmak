using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace InGame.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public partial class PlayerLookAround : MonoBehaviour
    {
        private PlayerInput _inputComponent;
        
        private async void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _inputComponent = gameObject.GetOrAddComponent<PlayerInput>();
            await UniTask.WaitUntil(() => _inputComponent.InputSystem != null);
            _inputComponent.AddValueListener("Look", LookAround);
        }
    }
    
    public partial class PlayerLookAround
    {
        [SerializeField] private Transform camArm;
        public Transform CamArm => camArm;

        private Vector2 _camView;

        [SerializeField] private float min;
        [SerializeField] private float max;
        private void LookAround(InputAction.CallbackContext context)
        {
            var mouse = context.ReadValue<Vector2>();
            _camView += mouse;
            _camView.x = ClampAngle(_camView.x, float.MinValue, float.MaxValue);
            _camView.y = ClampAngle(_camView.y, min, max);
            CamArm.rotation = Quaternion.Euler(_camView.y, _camView.x, 0);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}