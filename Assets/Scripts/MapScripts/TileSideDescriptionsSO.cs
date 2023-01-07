using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile side descriptor, it is used to prepare sides restriction per side kind
/// </summary>
[CreateAssetMenu]
public class TileSideDescriptionsSO: ScriptableObject
{
    /// <summary>
    /// Name of the tile side kind
    /// </summary>
    public SideDescription SideName;
    /// <summary>
    /// Tile side Restrictions
    /// </summary>
    public List<TileDictionaryPair> sideRestrictions;
}
