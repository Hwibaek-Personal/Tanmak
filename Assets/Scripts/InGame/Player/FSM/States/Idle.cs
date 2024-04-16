using InGame.Logic;

namespace InGame.Player.FSM.States
{
    public class PSIdle : PStateBase
    {
        private float _relaxTime;
        public PSIdle(PlayerState player) : base(player)
        {
            _relaxTime = 0;
        }

        public override void StateEnter()
        {
            Player.AirJumped = false;
            Player.Dashed = false;
            Player.IsJumping = false;
            Player.Gravity = 0;
            _relaxTime = 0;
        }

        public override void StateUpdate(float delta)
        {
            if (Player.Combat)
            {
                _relaxTime = 0;
                return;
            }
            if (_relaxTime > GameData.PlayerLogic.RelaxTime)
            {
                _relaxTime = GameData.PlayerLogic.RelaxTime;
                Player.CurrentHealth += GameData.PlayerLogic.RelaxValue;
                _relaxTime /= 2;
            }
            else
            {
                _relaxTime += delta;
            }
        }

        public override void StateExit()
        {
            _relaxTime = 0;
        }
    }
}