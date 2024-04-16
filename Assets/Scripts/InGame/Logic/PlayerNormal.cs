using UnityEngine;

namespace InGame.Logic
{
    [CreateAssetMenu]
    public class PlayerNormal : ScriptableObject
    {
        [Header("Status")]
        
        [SerializeField] private int health;
        public int Health => health;

        [SerializeField] private float relaxTime;
        public float RelaxTime => relaxTime;

        [SerializeField] private int relaxValue;
        public int RelaxValue => relaxValue;

        [SerializeField] private float speed;
        public float Speed => speed;

        [SerializeField] private float dashTime;
        public float DashTime => dashTime;
        
        [SerializeField] private float dashSpeed;
        public float DashSpeed => dashSpeed;

        [SerializeField] private float sternTime;
        public float SternTime => sternTime;
        
        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;
        
        [SerializeField] private float jumpTime;
        public float JumpTime => jumpTime;

        [SerializeField] private float landingPower;
        public float LandingPower => landingPower;
    }
}