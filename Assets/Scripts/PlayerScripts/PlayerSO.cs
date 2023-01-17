using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Players Scriptable object containing player stats
    /// </summary>
    [CreateAssetMenu]
    public class PlayerSO : ScriptableObject
    {
        /// <summary>
        /// Speed used for movement calculations
        /// </summary>
        public float speed = 1.4f;
        /// <summary>
        /// Sword attack power
        /// </summary>
        public float attack = 1.4f;
        /// <summary>
        /// Sword attack speed
        /// </summary>
        public float attackSpeed = 1.4f;
        /// <summary>
        /// Knockback distance modifier
        /// </summary>
        public float knocbackMultiplier = 1.4f;

        /// <summary>
        /// Determines if the speed was changed beside the items
        /// </summary>
        public bool isSpeedChanged = false;
        /// <summary>
        /// Base speed of the player
        /// </summary>
        public float baseSpeed = 1.4f;
        /// <summary>
        /// Base attack power of the player
        /// </summary>
        public float baseAttack = 1.4f;
        /// <summary>
        /// Base attack speed of the player
        /// </summary>
        public float baseAttackSpeed = 1.4f;
        /// <summary>
        /// Base knockback range of the player
        /// </summary>
        public float baseKnocbackMultiplier = 1.4f;

        /// <summary>
        /// Unused players equipment
        /// </summary>
        public List<ItemSO> equipment;
        /// <summary>
        /// Number of max items that can be equiped
        /// </summary>
        public int eqSpace = 4;
        /// <summary>
        /// Unused players inverntory
        /// </summary>
        public List<ItemSO> inventory;

        /// <summary>
        /// Current player health
        /// </summary>
        public float CurrentHealth = 5f;
        /// <summary>
        /// Maximum number of healthpoints
        /// </summary>
        public float MaxHealth = 5f;

        /// <summary>
        /// Current number of bombs at disposal
        /// </summary>
        public int secondaryAmmo = 5;
        /// <summary>
        /// Maximum number of bombs player can have
        /// </summary>
        public int maxSecondaryAmmo = 5;
    }
}
