using InGame.Logic;
using UnityEngine;

namespace InGame.Player.FSM.States
{
    public class PSDash : PStateBase
    {
        private float _dashTime;
        public bool IsDashComplete => _dashTime >= GameData.PlayerLogic.DashTime;
        private Vector3 _dir;
        public PSDash(PlayerState player) : base(player)
        {
            _dashTime = 0.0f;
        }

        public override void StateEnter()
        {
            _dir = Player.MovementComponent.Body.forward;
            _dashTime = 0.0f;
            Player.InputLock = true;
            Player.Dashed = true;
        }

        public override void StateUpdate(float delta)
        {
            _dashTime += delta;
            Player.Gravity = 0;
            Player.MovementComponent.Cc.Move(_dir * (GameData.PlayerLogic.DashSpeed * GameData.PlayerLogic.Speed * delta));
        }

        public override void StateExit()
        {
            _dashTime = 0.0f;
            Player.InputLock = false;
        }
    }
}