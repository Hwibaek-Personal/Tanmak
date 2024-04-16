using UnityEngine;
using UnityEngine.Serialization;

namespace InGame.Logic
{
    public class LogicSo : ScriptableObject
    {
        [SerializeField, Min(3)] private int mapSize;
        public int MapSize => mapSize;

        [SerializeField, Min(0)] private float combatTime;
        public float CombatTime => combatTime;

        [SerializeField, Min(0)] private float groundCheckRange;
        public float GroundCheckRange => groundCheckRange;
        
        [SerializeField] private LayerMask groundLayer;
        public LayerMask GroundLayer => groundLayer;
        
        [SerializeField] private LayerMask enemyLayer;
        public LayerMask EnemyLayer => enemyLayer;

        [SerializeField] private float gravityScale;
        public float GravityScale => gravityScale;

        [SerializeField] private float maxFallSpeed;
        public float MaxFallSpeed => maxFallSpeed;

        
    }
}