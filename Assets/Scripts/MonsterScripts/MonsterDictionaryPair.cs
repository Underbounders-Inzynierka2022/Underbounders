using System;

/// <summary>
/// Dictionary holding monster type and its probability
/// </summary>
[Serializable]
public class MonsterDictionaryPair
{
    /// <summary>
    /// Monster type to be restricted
    /// </summary>
    public MonsterType monster;
    /// <summary>
    /// Chance modifier
    /// </summary>
    public float chance;
}
