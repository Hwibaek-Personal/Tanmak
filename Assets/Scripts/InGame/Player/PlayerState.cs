using System;
using Cysharp.Threading.Tasks;
using InGame.Logic;
using InGame.Player.FSM;
using InGame.Player.FSM.States;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace InGame.Player
{
    [RequireComponent(
        typeof(PlayerEventHandler),
        typeof(PlayerInput),
        typeof(PlayerMovement)
        )
    ]
    [DisallowMultipleComponent]
    public partial class PlayerState : MonoBehaviour
    {
        public PlayerEventHandler EventComponent { get; private set; }
        public PlayerMovement MovementComponent { get; private set; }
        public PlayerInput InputComponent { get; private set; }

        private void Awake()
        {
            EventComponent = gameObject.GetOrAddComponent<PlayerEventHandler>();
            MovementComponent = gameObject.GetOrAddComponent<PlayerMovement>();
            InputComponent = gameObject.GetOrAddComponent<PlayerInput>();
        }
    }

    public partial class PlayerState
    {
        [SerializeField] private Transform groundCheckPos;
        public Transform GroundCheckPos => groundCheckPos;
    }

    public partial class PlayerState
    {
        [SerializeField] private PState c;
        public PState Current { get => c; private set => c = value; }
        public PSFsm Fsm { get; private set; }

        private void Start()
        {
            Current = PState.Idle;
            Fsm = new PSFsm(new PSIdle(this));
        }

        private void ChangeState(PState state)
        {
            Current = state;
            switch (Current)
            {
                case PState.Idle:
                    Fsm.ChangeState(new PSIdle(this));
                    break;
                case PState.Jump:
                    Fsm.ChangeState(new PSJump(this));
                    break;
                case PState.AirJump:
                    Fsm.ChangeState(new PSAirJump(this));
                    break;
                case PState.Dash:
                    Fsm.ChangeState(new PSDash(this));
                    break;
                case PState.Falling:
                    Fsm.ChangeState(new PSFalling(this));
                    break;
                case PState.Landing:
                    Fsm.ChangeState(new PSLanding(this));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            switch (Current)
            {
                case PState.Idle:
                {
                    if (!IsGrounded())
                    {
                        ChangeState(PState.Falling);
                    }
                    else if (InputComponent.GetButtonDown("Jump") && !InputLock)
                    {
                        ChangeState(PState.Jump);
                    }
                    else if (InputComponent.GetButtonDown("Dash") && !InputLock)
                    {
                        ChangeState(PState.Dash);
                    }
                    break;
                }
                case PState.Jump:
                {
                    if (InputComponent.GetButtonUp("Jump") || !IsJumping)
                    {
                        ChangeState(PState.Falling);
                    }
                    break;
                }
                case PState.AirJump:
                {
                    if (InputComponent.GetButtonDown("Jump") && !InputLock)
                    {
                        ChangeState(PState.Landing);
                    }
                    else if (!IsJumping)
                    {
                        ChangeState(PState.Falling);
                    }
                    break;
                }
                case PState.Dash:
                {
                    if (Fsm.CurrentState is PSDash { IsDashComplete: true })
                    {
                        ChangeState(PState.Falling);
                    }
                    break;
                }
                case PState.Falling:
                {
                    if (IsGrounded())
                    {
                        ChangeState(PState.Idle);
                    }
                    else if (InputComponent.GetButtonDown("Jump") && !InputLock)
                    {
                        ChangeState(!AirJumped ? PState.AirJump : PState.Landing);
                    }
                    else if (InputComponent.GetButtonDown("Dash") && !Dashed && !InputLock)
                    {
                        ChangeState(PState.Dash);
                    }
                    break;
                }
                case PState.Landing:
                {
                    if (IsGrounded())
                    {
                        ChangeState(PState.Idle);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Fsm.Update(Time.deltaTime);
        }
    }

    public partial class PlayerState
    {
        public bool Combat { get; private set; }
        public float CombatTime { get; private set; }

        public async UniTaskVoid EnterCombat()
        {
            Combat = true;
            CombatTime = GameData.Logic.CombatTime;
            while (CombatTime > 0)
            {
                CombatTime -= Time.deltaTime;
                await UniTask.WaitForSeconds(Time.deltaTime);
            }
            Combat = false;
            CombatTime = 0;
        }

        private float _sternTime;
        

        public async UniTaskVoid EnterStern()
        {
            InputLock = true;
            MovementComponent.moveVec = Vector3.zero;
            _sternTime = GameData.PlayerLogic.SternTime;
            while (_sternTime > 0)
            {
                _sternTime -= Time.deltaTime;
                await UniTask.WaitForSeconds(Time.deltaTime);
            }
            InputLock = false;
            _sternTime = 0;
        }
        
        public bool IsJumping { get; set; }
        public bool InputLock { get; set; }
        public bool Dashed { get; set; }
        public bool AirJumped { get; set; }

        public bool IsGrounded() => Physics.OverlapSphere(GroundCheckPos.position, GameData.Logic.GroundCheckRange, GameData.Logic.GroundLayer).Length > 0;

        [SerializeField] private float gravity;
        public float Gravity
        {
            get => gravity;
            set => gravity = Mathf.Max(value, -(c == PState.Landing ? GameData.Logic.MaxFallSpeed * GameData.PlayerLogic.LandingPower : GameData.Logic.MaxFallSpeed));
        }
    }
    public partial class PlayerState
    {
        private int _maxHealth;
        
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                CurrentHealth = CurrentHealth;
            }
        }
        
        private int _currentHealth;
        
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                var old = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth < old)
                {
                    EventComponent.OnDamage.Invoke(old - _currentHealth);
                    if (_currentHealth <= 0)
                    {
                        EventComponent.OnDeath.Invoke();
                    }
                }
                else if (_currentHealth > old)
                {
                    EventComponent.OnHeal.Invoke(_currentHealth - old);
                }
            }
        }
    }
}