using System;
using UnityEngine;

namespace MapScripts.Obstacles
{
    /// <summary>
    /// Spawn possibility container
    /// </summary>
    [Serializable]
    public class ItemSpawnRate
    {
        /// <summary>
        /// Item to spawn from chest
        /// </summary>
        public GameObject item;
        /// <summary>
        /// Possibility of item spawning
        /// </summary>
        public float chance;
    }
}