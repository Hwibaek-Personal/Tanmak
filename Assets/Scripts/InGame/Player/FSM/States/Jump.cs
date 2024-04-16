using InGame.Logic;
using UnityEngine;

namespace InGame.Player.FSM.States
{
    public class PSJump : PStateBase
    {
        private float _timer;
        public PSJump(PlayerState player) : base(player)
        {
            _timer = 0f;
        }

        public override void StateEnter()
        {
            _timer = 0f;
            Player.IsJumping = true;
        }

        public override void StateUpdate(float delta)
        {
            if (_timer < GameData.PlayerLogic.JumpTime)
            {
                _timer += delta;
                Player.Gravity = GameData.PlayerLogic.JumpForce;
            }
            else
            {
                Player.IsJumping = false;
            }
        }

        public override void StateExit()
        {
            _timer = 0f;
            Player.Gravity *= 0.5f;
            Player.IsJumping = false;
        }
    }
}