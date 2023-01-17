using System;
using System.Collections.Generic;

namespace SaveSystem
{
    /// <summary>
    /// Class holding save data of the game
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// Current save file data instance
        /// </summary>
        private static SaveData s_current;
        /// <summary>
        /// Public getter and setter for singleton instance of save file
        /// </summary>
        public static SaveData Current
        {
            get
            {
                if (s_current is null)
                {
                    s_current = new SaveData();
                }
                return s_current;
            }

            set
            {
                s_current = value;
            }
        }

        /// <summary>
        /// PlayerData to save
        /// </summary>
        public PlayerDataSave Player { get; set; }
        /// <summary>
        /// Seed of the world
        /// </summary>
        public int Seed { get; set; }
        /// <summary>
        /// Rooms with their seeds, chests and conquers
        /// </summary>
        public List<RoomsDataSave> Rooms { get; set; }
        /// <summary>
        /// Current room position in matrix
        /// </summary>
        public (int x, int y) CurrentRoom { get; set; }
        /// <summary>
        /// Current timer state
        /// </summary>
        public float Timer { get; set; }
    }
}
