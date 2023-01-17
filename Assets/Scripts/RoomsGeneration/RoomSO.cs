using MapScripts;
using System.Collections.Generic;
using UnderBounders;
using UnityEngine;

namespace RoomGeneration
{
    /// <summary>
    /// Scriptable object representing kind of room by describing its sides
    /// </summary>
    [CreateAssetMenu]
    public class RoomSO : ScriptableObject, ITileKind<RoomSO>
    {
        /// <summary>
        /// Determines if there are doors up
        /// </summary>
        public bool IsDoorUp;
        /// <summary>
        /// Determines if there are doors down
        /// </summary>
        public bool IsDoorDown;
        /// <summary>
        /// Determines if there are doors left
        /// </summary>
        public bool IsDoorLeft;
        /// <summary>
        /// Determines if there are doors right
        /// </summary>
        public bool IsDoorRight;


        /// <summary>
        /// Filteres tiles from certain neighbouring field
        /// </summary>
        /// <param name="tilesToFilter">
        /// Tiles posiblities aligned in neighborhood
        /// </param>
        /// <param name="dir">
        /// Determines which side is considered in comparations
        /// </param>
        /// <returns>
        /// Filtered rooms list
        /// </returns>
        public Dictionary<RoomSO, float> FilterTiles(Dictionary<RoomSO, float> tilesToFilter, Direction dir)
        {
            var filtered = new Dictionary<RoomSO, float>();
            foreach (var tile in tilesToFilter.Keys)
            {
                switch (dir)
                {
                    case Direction.down:
                        if (tile.IsDoorUp == IsDoorDown)
                            filtered.Add(tile, tilesToFilter[tile]);
                        else
                            filtered.Add(tile, 0);
                        break;
                    case Direction.up:
                        if (tile.IsDoorDown == IsDoorUp)
                            filtered.Add(tile, tilesToFilter[tile]);
                        else
                            filtered.Add(tile, 0);
                        break;
                    case Direction.left:
                        if (tile.IsDoorRight == IsDoorLeft)
                            filtered.Add(tile, tilesToFilter[tile]);
                        else
                            filtered.Add(tile, 0);
                        break;
                    case Direction.right:
                        if (tile.IsDoorLeft == IsDoorRight)
                            filtered.Add(tile, tilesToFilter[tile]);
                        else
                            filtered.Add(tile, 0);
                        break;
                    default:
                        filtered.Add(tile, tilesToFilter[tile]);
                        break;
                }
            }
            tilesToFilter = filtered;
            return filtered;
        }
    }
}
