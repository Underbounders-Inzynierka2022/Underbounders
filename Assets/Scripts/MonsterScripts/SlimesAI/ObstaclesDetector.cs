using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class detecting obstacles in slime vicinity
/// </summary>
public class ObstaclesDetector : Detector
{
    /// <summary>
    /// Radius in which obsctacle can be detected
    /// </summary>
    [SerializeField] private float detectionRadius = 0.5f;
    /// <summary>
    /// Layer mask describing which colliders should be detected
    /// </summary>
    [SerializeField] private LayerMask layerMask;
    /// <summary>
    /// Determinator if gizmos should be displayed in editor
    /// </summary>
    [SerializeField] private bool showGizmos = true;

    /// <summary>
    /// Detected colliders
    /// </summary>
    private Collider2D[] _colliders;

    /// <summary>
    /// Detects all colliders from obstacles layer mask
    /// </summary>
    /// <param name="aiData">
    /// Current targets and obstacles
    /// </param>
    public override void Detect(AIData aiData)
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.Obstacles = _colliders;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        if(Application.isPlaying && _colliders != null)
        {
            Gizmos.color = Color.magenta;
            foreach(var col in _colliders)
            {
                Gizmos.DrawSphere(col.transform.position, 0.002f);
            }
        }
    }
}
