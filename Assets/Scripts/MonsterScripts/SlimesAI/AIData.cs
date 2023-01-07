using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Targets and obstacles positions container
/// </summary>
public class AIData: MonoBehaviour
{
    /// <summary>
    /// Current list of targets
    /// </summary>
    public List<Transform> Targets { get; set; }
    /// <summary>
    /// Current obstacles in monster vicinity
    /// </summary>
    public Collider2D[] Obstacles { get; set; }
    /// <summary>
    /// Current target
    /// </summary>
    public Transform CurrentTarget { get; set; }
    /// <summary>
    /// Provides number of targets with null handling
    /// </summary>
    /// <returns>
    /// Number of targets available
    /// </returns>
    public int GetTargetsCount() => Targets is null ? 0 : Targets.Count;
}
