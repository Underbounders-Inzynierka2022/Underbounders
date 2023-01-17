using MonsterScripts;
using System.Collections.Generic;
using System.Linq;
using UnderBounders;
using UnityEngine;

namespace MapScripts.Obstacles
{
    /// <summary>
    /// Tile representation of obstacles
    /// </summary>
    [CreateAssetMenu]
    public class ObstacleSO : ScriptableObject, ITileKind<ObstacleSO>
    {
        /// <summary>
        /// Obstacle
        /// </summary>
        public UnityEngine.Object tile;

        /// <summary>
        /// Type of obstacle
        /// </summary>
        public ObstacleType obstacleType;

        /// <summary>
        /// Restrictions to upside adjecent tile
        /// </summary>
        public List<ObstacleDictionaryPair> UpRestrictions;
        /// <summary>
        /// Restrictions to left side adjecent tile
        /// </summary>
        public List<ObstacleDictionaryPair> DownRestrictions;
        /// <summary>
        /// Restrictions to downside adjecent tile
        /// </summary>
        public List<ObstacleDictionaryPair> LeftRestrictions;
        /// <summary>
        /// Restrictions to right side adjecent tile
        /// </summary>
        public List<ObstacleDictionaryPair> RightRestrictions;

        /// <summary>
        /// List of monsters spawnrates on the obstacle
        /// </summary>
        public List<MonsterDictionaryPair> spawnableMonsters;

        /// <summary>
        /// Filters obstacle possiblities in its particullar neighbourhood
        /// </summary>
        /// <param name="tilesToFilter">
        /// TObstacles possibilities that needs filtering
        /// </param>
        /// <param name="dir">
        /// Obstacle tile adjency
        /// </param>
        /// <returns>
        /// Filtered obstacles possiblities
        /// </returns>
        public Dictionary<ObstacleSO, float> FilterTiles(Dictionary<ObstacleSO, float> tilesToFilter, Direction dir)
        {
            Dictionary<ObstacleSO, float> filtered = new Dictionary<ObstacleSO, float>();
            foreach (var tile in tilesToFilter.Keys)
            {
                switch (dir)
                {
                    case Direction.down:
                        if (DownRestrictions.Any(r => r.obstacle == tile.obstacleType))
                        {
                            float chance = DownRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                            filtered.Add(tile, tilesToFilter[tile] * chance);
                        }
                        break;
                    case Direction.right:
                        if (UpRestrictions.Any(r => r.obstacle == tile.obstacleType))
                        {
                            float chance = RightRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                            filtered.Add(tile, tilesToFilter[tile] * chance);
                        }
                        break;
                    case Direction.left:
                        if (RightRestrictions.Any(r => r.obstacle == tile.obstacleType))
                        {
                            float chance = LeftRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                            filtered.Add(tile, tilesToFilter[tile] * chance);
                        }
                        break;
                    case Direction.up:
                        if (UpRestrictions.Any(r => r.obstacle == tile.obstacleType))
                        {
                            float chance = UpRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                            filtered.Add(tile, tilesToFilter[tile] * chance);
                        }
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