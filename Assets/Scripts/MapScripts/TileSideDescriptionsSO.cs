using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileSideDescriptionsSO: ScriptableObject
{
    public SideDescription SideName;

    public List<TileDictionaryPair> sideRestrictions;
}
