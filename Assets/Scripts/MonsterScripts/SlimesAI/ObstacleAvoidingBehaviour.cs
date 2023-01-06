using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidingBehaviour : SteeringBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float agentColliderSize = 0.6f;

    [SerializeField] private bool showGizmo = true;

    float[] dangersResultTemp = null;

    public override (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData)
    {
        foreach(var col in aiData.Obstacles)
        {
            Vector2 dirToObs = col.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distToObs = dirToObs.magnitude;

            float weight = distToObs <= agentColliderSize ? 1 : (radius - distToObs) / radius;
            Vector2 dirNormalized = dirToObs.normalized;

            for(int i = 0; i<Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(dirNormalized, Directions.eightDirections[i]);

                float valueToPutIn = result * weight;

                if(valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, intrest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        if (Application.isPlaying && dangersResultTemp != null)
        {
            Gizmos.color = Color.magenta;
            for(int i =0; i<dangersResultTemp.Length;i++)
            {
                Gizmos.DrawRay((Vector2)transform.position, Directions.eightDirections[i] * dangersResultTemp[i]);
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
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
