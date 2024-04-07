using UnityEngine;
using UnrealTeam.SB.Common.Constants;

namespace UnrealTeam.SB.GamePlay.AI.Behaviours
{
    public class HungerBehaviour : MonoBehaviour
    {
        public float Hunger { get; set; }
        public float MaxHunger { get; set; }
        public bool IsDepleting { get; set; } = true;
        public float DepletionRate { get; set; }


        private void Update()
        {
            if (!IsDepleting || IsFullyHunger())
                return;
            
            Hunger += DepletionRate * Time.deltaTime;
        }

        private bool IsFullyHunger() 
            => MaxHunger - Hunger < GameConstants.Tolerance;
    }
}