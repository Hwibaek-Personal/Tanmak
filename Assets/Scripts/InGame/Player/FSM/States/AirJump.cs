using InGame.Logic;

namespace InGame.Player.FSM.States
{
    public class PSAirJump : PStateBase
    {
        private float _timer;
        public PSAirJump(PlayerState player) : base(player)
        {
            _timer = 0;
        }

        public override void StateEnter()
        {
            Player.AirJumped = true;
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
            Player.IsJumping = false;
            _timer = 0;
        }
    }
}