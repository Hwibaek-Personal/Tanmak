using System.Collections.Generic;
using UnityEngine;

namespace InGame.Player.FSM
{
    public abstract class PStateBase
    {
        protected PlayerState Player;
        protected PStateBase(PlayerState player)
        {
            Player = player;
        }

        public abstract void StateEnter();
        public abstract void StateUpdate(float delta);
        public abstract void StateExit();
    }

    public class PSFsm
    {
        public PStateBase CurrentState { get; private set; }
        public PSFsm(PStateBase init)
        {
            CurrentState = init;
            ChangeState(CurrentState);
        }

        public void ChangeState(PStateBase next)
        {
            if (next == CurrentState) return;
            CurrentState?.StateExit();
            CurrentState = next;
            CurrentState?.StateEnter();
        }
        
        public void Update(float delta)
        {
            CurrentState?.StateUpdate(delta);
        }
    }
}