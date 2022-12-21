using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomSO : ScriptableObject, ITileKind<RoomSO>
{
    public bool IsDoorUp;
    public bool IsDoorDown;
    public bool IsDoorLeft;
    public bool IsDoorRight;

    

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
                    break;
                case Direction.up:
                    if (tile.IsDoorDown == IsDoorUp)
                        filtered.Add(tile, tilesToFilter[tile]);
                    break;
                case Direction.left:
                    if (tile.IsDoorRight == IsDoorLeft)
                        filtered.Add(tile, tilesToFilter[tile]);
                    break;
                case Direction.right:
                    if (tile.IsDoorLeft == IsDoorRight)
                        filtered.Add(tile, tilesToFilter[tile]);
                    break;
                default:
                    filtered.Add(tile, tilesToFilter[tile]);
                    break;
            }
        }
        return filtered;
    }
}
