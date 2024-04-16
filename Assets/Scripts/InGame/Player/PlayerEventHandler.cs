using System;
using UnityEngine;

namespace InGame.Player
{
    [DisallowMultipleComponent]
    public partial class PlayerEventHandler : MonoBehaviour
    {
        public Action<int> OnDamage;
        public Action<int> OnHeal;
        public Action OnDeath;
    }
}