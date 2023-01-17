using System.Collections.Generic;

namespace RoomGeneration
{
    /// <summary>
    /// Class containing all rooms data from particular room in the dungeon
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room collumn in the matrix
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// Room row in the matrix
        /// </summary>
        public int y { get; set; }
        /// <summary>
        /// Random generation seed of the room
        /// </summary>
        public int Seed { get; set; }
        /// <summary>
        /// List of the opened chests positions
        /// </summary>
        public List<(int x, int y)> ChestOpened { get; set; }
        /// <summary>
        /// Room kind description containing rooms
        /// </summary>
        public RoomSO RoomKind { get; set; }
        /// <summary>
        /// Determines if room is conquered
        /// </summary>
        public bool IsConquered { get; set; }
        /// <summary>
        /// This constructor creates new unconquered room
        /// </summary>
        /// <param name="i">
        /// Room collumn in the matrix
        /// </param>
        /// <param name="j">
        /// Room row in the matrix
        /// </param>
        /// <param name="roomKind">
        /// Initilize roomKind for doors
        /// </param>
        /// <param name="seed">
        /// Room seed for map generation
        /// </param>
        public Room(int i, int j, RoomSO roomKind, int seed)
        {
            x = i;
            y = j;
            this.RoomKind = roomKind;
            this.Seed = seed;
            IsConquered = false;
            ChestOpened = new List<(int, int)>();
        }
        /// <summary>
        /// Translates invidual determinates of the doors into array
        /// </summary>
        /// <returns>
        /// Array with the doors determinates in order: right, up, left, down
        /// </returns>
        public bool[] GetRoomsDoorsAsArray()
        {
            var arr = new bool[4];
            arr[0] = RoomKind.IsDoorRight;
            arr[1] = RoomKind.IsDoorUp;
            arr[2] = RoomKind.IsDoorLeft;
            arr[3] = RoomKind.IsDoorDown;
            return arr;
        }
    }
}
