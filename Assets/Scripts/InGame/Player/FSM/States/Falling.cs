using UnityEngine;

namespace InGame.Player.FSM.States
{
    public class PSFalling : PStateBase
    {
        public PSFalling(PlayerState player) : base(player)
        {
            
        }

        public override void StateEnter()
        {
            
        }

        public override void StateUpdate(float delta)
        {
            Player.Gravity += Physics.gravity.y * delta;
        }

        public override void StateExit()
        {
            
        }
    }
}