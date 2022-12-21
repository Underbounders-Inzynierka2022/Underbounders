using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDetector : Detector
{
    [SerializeField] private float detectionRadius = 0.5f;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private bool showGizmos = true;

    private Collider2D[] colliders;

    public override void Detect(AIData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        if(Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.magenta;
            foreach(var col in colliders)
            {
                Gizmos.DrawSphere(col.transform.position, 0.002f);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
