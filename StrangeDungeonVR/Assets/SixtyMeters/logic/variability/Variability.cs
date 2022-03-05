using System;
using UnityEngine;

namespace SixtyMeters.logic.variability
{
    
    [Serializable]
    public class Variability
    {
        [Tooltip("Player health at the start of the game")]
        public int playerBaseHealth;

        [Tooltip("The base damage of a sword hit")]
        public int swordBaseDamage;

        [Tooltip("Damage dealt by the player is multiplied by this")]
        public int damageDealtMultiplier;

        [Tooltip("Damage taken by the player is multiplied by this")]
        public int damageTakenMultiplier;

    }
}