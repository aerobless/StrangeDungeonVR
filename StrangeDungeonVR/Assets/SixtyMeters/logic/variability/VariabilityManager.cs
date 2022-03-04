using System.Collections.Generic;
using SixtyMeters.logic.variability.effects;
using UnityEngine;

namespace SixtyMeters.logic.variability
{
    /// <summary>
    /// The VariabilityManager stores all variables settings of the game. This includes things like damage modifiers,
    /// light brightness, player hp regen, etc. These settings are modified during gameplay by activating effects that
    /// the player finds in the game world. E.g. an effect might multiple his damage dealt by 2 but also increase damage
    /// taken by 2. Or another modifier might reduce fall damage from a certain height.
    /// </summary>
    public class VariabilityManager : MonoBehaviour
    {
        // Internals
        private readonly List<VariabilityEffect> _appliedEffects = new();

        // Player
        [Tooltip("Player health at the start of the game")]
        public int playerBaseHealth;

        [Tooltip("The base damage of a sword hit")]
        public int swordBaseDamage;

        [Tooltip("Damage dealt by the player is multiplied by this")]
        public int damageDealtMultiplier;

        [Tooltip("Damage taken by the player is multiplied by this")]
        public int damageTakenMultiplier;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void AddAppliedEffect(VariabilityEffect effect)
        {
            _appliedEffects.Add(effect);
            Debug.Log("Applied effect " + effect.GetName());
        }

        public void RemoveAppliedEffect(VariabilityEffect effect)
        {
            _appliedEffects.Remove(effect);
            Debug.Log("Removed effect " + effect.GetName());
        }
    }
}