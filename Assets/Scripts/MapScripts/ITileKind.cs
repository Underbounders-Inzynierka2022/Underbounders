using System.Collections.Generic;

public interface ITileKind<T>
{
    public Dictionary<T, float> FilterTiles(Dictionary<T, float> tileTofilter, Direction dir);
}