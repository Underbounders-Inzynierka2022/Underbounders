using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float detectionRadius = 2;

    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;

    [SerializeField] private bool showGizmos = false;

    private List<Transform> targets;

    public override void Detect(AIData aiData)
    {
        Collider2D playerColl  = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayerMask);

        if(playerColl != null)
        {
            Vector2 dir = (playerColl.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionRadius, obstacleLayerMask);

            //Debug.DrawRay(transform.position, dir * detectionRadius, Color.cyan);

            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                
                targets = new List<Transform>() { playerColl.transform };
            }
            else
            {
                targets = null;
            }

        }
        else
        {
            targets = null;
        }
        aiData.targets = targets;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        if (Application.isPlaying && targets != null && targets.Count > 0)
        {
            Gizmos.color = Color.magenta;
            foreach (var col in targets)
            {
                Gizmos.DrawSphere(col.transform.position, 0.002f);
            }
        }
    }
}
