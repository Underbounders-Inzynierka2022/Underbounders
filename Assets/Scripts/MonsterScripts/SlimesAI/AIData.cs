using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Targets and obstacles positions container
/// </summary>
public class AIData: MonoBehaviour
{
    public List<Transform> Targets { get; set; }
    public Collider2D[] Obstacles { get; set; }

    public Transform CurrentTarget { get; set; }

    public int GetTargetsCount() => Targets is null ? 0 : Targets.Count;
}
