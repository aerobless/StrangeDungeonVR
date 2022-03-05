using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SixtyMeters.logic.utilities;
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

        [Serializable]
        public struct PlayerVar
        {
            [Tooltip("Player health at the start of the game")]
            public int baseHealth;

            [Tooltip("The base damage of a sword hit")]
            public int swordBaseDamage;

            [Tooltip("Damage dealt by the player is multiplied by this")]
            public int damageDealtMultiplier;

            [Tooltip("Damage taken by the player is multiplied by this")]
            public int damageTakenMultiplier;
        }

        [SerializeField] public PlayerVar player;

        // Backup Variability when the player needs to be restored to its initial settings
        private PlayerVar _initialPlayerVar;

        // Start is called before the first frame update
        void Start()
        {
            _initialPlayerVar = Helper.CreateDeepCopy(player);
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

        /// <summary>
        /// Restores the variability to the games default values
        /// </summary>
        public void RestoreInitialVariability()
        {
            player = Helper.CreateDeepCopy(_initialPlayerVar);
            _appliedEffects.Clear();
        }
    }
}