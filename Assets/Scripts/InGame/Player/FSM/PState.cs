namespace InGame.Player.FSM
{
    public enum PState
    {
        /// <summary>
        /// 대기 상태
        /// </summary>
        Idle,
        /// <summary>
        /// 땅점프 상태
        /// </summary>
        Jump,
        /// <summary>
        /// 공중점프 상태
        /// </summary>
        AirJump,
        /// <summary>
        /// 대쉬 상태
        /// </summary>
        Dash,
        /// <summary>
        /// 낙하 상태
        /// </summary>
        Falling,
        /// <summary>
        /// 급강하 상태
        /// </summary>
        Landing
    }
}