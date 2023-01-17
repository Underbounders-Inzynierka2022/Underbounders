using System;

namespace MonsterScripts
{
    /// <summary>
    /// Monster spawn chance modifier with its monster type
    /// </summary>
    [Serializable]
    public class SpawnRate
    {
        /// <summary>
        /// Monster type to spawn
        /// </summary>
        public MonsterType spawn;

        /// <summary>
        /// Propability modifier
        /// </summary>
        public float chance;

    }
}
