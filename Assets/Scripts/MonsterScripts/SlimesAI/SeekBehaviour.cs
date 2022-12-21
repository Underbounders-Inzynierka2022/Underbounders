using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private float threashold = 0.5f;

    [SerializeField] private bool showGizmo = true;


    bool reachedLastTarget = true;
    private Vector2 targetPositionCache;
    float[] interestResultTemp = null;

    public override (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData)
    {
        if (reachedLastTarget)
        {
            if(aiData.targets is null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = null;
                return (danger, intrest);
            }
            reachedLastTarget = false;
            aiData.currentTarget = aiData.targets.OrderBy(t => Vector2.Distance(t.position, transform.position)).FirstOrDefault();
        }

        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
            targetPositionCache = aiData.currentTarget.position;

        if(Vector2.Distance(transform.position, targetPositionCache) < 0.1f)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, intrest);
        }

        Vector2 dirToTarget = (targetPositionCache - (Vector2)transform.position);
        for(int i = 0; i < intrest.Length; i++)
        {
            float result = Vector2.Dot(dirToTarget.normalized, Directions.eightDirections[i]);

            if(result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > intrest[i])
                    intrest[i] = valueToPutIn;
            }
        }
        interestResultTemp = intrest;
        return (danger, intrest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        Gizmos.DrawWireSphere(targetPositionCache, 0.1f);

        if (Application.isPlaying && interestResultTemp != null)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < interestResultTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestResultTemp[i]);
            }
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
    

    public static class Directions
    {
        public static List<Vector2> eightDirections = new List<Vector2>()
        {
            new Vector2(0,1).normalized,
            new Vector2(1,1).normalized,
            new Vector2(1,0).normalized,
            new Vector2(1,-1).normalized,
            new Vector2(0,-1).normalized,
            new Vector2(-1,-1).normalized,
            new Vector2(-1,0).normalized,
            new Vector2(-1,1).normalized
        };
    }
}
