using InGame.Logic;
using UnityEngine;

namespace InGame.Player.FSM.States
{
    public class PSLanding : PStateBase
    {
        public PSLanding(PlayerState player) : base(player)
        {
            
        }

        public override void StateEnter()
        {
            Player.InputLock = true;
        }

        public override void StateUpdate(float delta)
        {
            Player.Gravity += Physics.gravity.y * delta * GameData.PlayerLogic.LandingPower;
        }

        public override void StateExit()
        {
            Player.InputLock = false;
            Player.EnterStern().Forget();
        }
    }
}