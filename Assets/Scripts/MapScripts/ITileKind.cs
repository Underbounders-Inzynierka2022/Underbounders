using System.Collections.Generic;

/// <summary>
/// Interface allowing the class to be used in map matrix class
/// </summary>
/// <typeparam name="T">
/// Tile type
/// </typeparam>
public interface ITileKind<T>
{
    /// <summary>
    /// It filters tiles in certain neighbourhood
    /// </summary>
    /// <param name="tileTofilter">
    /// Tiles possibilities of neighbouring tile
    /// </param>
    /// <param name="dir">
    /// Defines direction distincting restrictions of the tile
    /// </param>
    /// <returns>
    /// Filtered tiles posibilities
    /// </returns>
    public Dictionary<T, float> FilterTiles(Dictionary<T, float> tileTofilter, Direction dir);
}