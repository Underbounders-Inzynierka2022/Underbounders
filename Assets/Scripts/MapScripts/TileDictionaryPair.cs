using System;

/// <summary>
/// Tiles side propability modifier
/// </summary>
[Serializable]
public class TileDictionaryPair 
{
    /// <summary>
    /// Name of the side of the tile
    /// </summary>
    public SideDescription kindOfSide;
    /// <summary>
    /// Propability modifier
    /// </summary>
    public float chance;
}
