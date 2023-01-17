using System;

namespace MapScripts.Tiles
{
    [Serializable]
    public class KindOfFloorConstraint
    {
        public TileKindName tileKind;
        public float chanceMod;
    }
}