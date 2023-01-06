using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detector implementation for targets
/// </summary>
public class TargetDetector : Detector
{
    [SerializeField] private float detectionRadius = 2;
    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;
    [SerializeField] private bool showGizmos = false;

    private List<Transform> _targets;

    /// <summary>
    /// Detects player in monster vicinity
    /// </summary>
    /// <param name="aiData">
    /// Data stored for the ai
    /// </param>
    public override void Detect(AIData aiData)
    {
        Collider2D playerColl  = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayerMask);

        if(playerColl != null)
        {
            Vector2 dir = (playerColl.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionRadius, obstacleLayerMask);

            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                _targets = new List<Transform>() { playerColl.transform };
            }
            else
            {
                _targets = null;
            }

        }
        else
        {
            _targets = null;
        }
        aiData.Targets = _targets;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        if (Application.isPlaying && _targets != null && _targets.Count > 0)
        {
            Gizmos.color = Color.magenta;
            foreach (var col in _targets)
            {
                Gizmos.DrawSphere(col.transform.position, 0.002f);
            }
        }
    }
}
