using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Unused items Scriptable object, left for future
    /// </summary>
    [CreateAssetMenu]
    public class ItemSO : ScriptableObject
    {
        /// <summary>
        /// Additional attack value of the item
        /// </summary>
        public int attackStat;
        /// <summary>
        /// Additional attack speed value of the item
        /// </summary>
        public int attackSpeedStat;
        /// <summary>
        /// Additional speed value of the item
        /// </summary>
        public int speedStat;
        /// <summary>
        /// Scrapped defence stat
        /// </summary>
        public int defenceStat;

        /// <summary>
        /// Item sprite
        /// </summary>
        public Sprite sprite;
        /// <summary>
        /// Unused name of the item kind
        /// </summary>
        public string blessing;

    }
}
