using System;
using InGame.Player.FSM;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace InGame.Player
{
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput), typeof(PlayerState))]
    public partial class PlayerInput : MonoBehaviour
    {
        public UnityEngine.InputSystem.PlayerInput InputSystem { get; private set; }
        private PlayerState _stateComponent;

        private void Awake()
        {
            InputSystem = gameObject.GetOrAddComponent<UnityEngine.InputSystem.PlayerInput>();
            _stateComponent = gameObject.GetOrAddComponent<PlayerState>();
        }
    }
    
    public partial class PlayerInput
    {
        public void AddListener(string actName, Action<InputAction.CallbackContext> performedAct)
        {
            InputSystem.actions[actName].performed += performedAct;
        }

        public void AddValueListener(string actName, Action<InputAction.CallbackContext> performedAct)
        {
            InputSystem.actions[actName].started += performedAct;
            InputSystem.actions[actName].canceled += performedAct;
            InputSystem.actions[actName].performed += performedAct;
        }

        public void AddListener(string actName, Action<InputAction.CallbackContext> performedAct, PStateBase change)
        {
            InputSystem.actions[actName].performed += cc =>
            {
                performedAct.Invoke(cc);
                _stateComponent.Fsm.ChangeState(change);
            };
        }

        public bool GetButtonDown(string actName) => InputSystem.actions[actName].WasPressedThisFrame();

        public bool GetButton(string actName) => InputSystem.actions[actName].IsPressed();

        public bool GetButtonUp(string actName) => InputSystem.actions[actName].WasReleasedThisFrame();
    }
}