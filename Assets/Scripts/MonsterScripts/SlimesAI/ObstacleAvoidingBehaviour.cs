using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidingBehaviour : SteeringBehaviour
{
    /// <summary>
    /// Obstacle maximum distance taken into consderation
    /// </summary>
    [SerializeField] private float radius = 2f;
    /// <summary>
    /// Size of monster collider
    /// </summary>
    [SerializeField] private float agentColliderSize = 0.6f;
    /// <summary>
    /// Debugging options if the gizmos should be visible in editor
    /// </summary>
    [SerializeField] private bool showGizmo = true;

    /// <summary>
    /// Cached obstacles in vicinity
    /// </summary>
    float[] dangersResultTemp = null;

    /// <summary>
    /// Calculates steering for all of the obstacles
    /// </summary>
    /// <param name="danger">
    /// Current danger values
    /// </param>
    /// <param name="intrest">
    /// Current intrest values
    /// </param>
    /// <param name="aiData">
    /// AI data containing all of the targets and colliders
    /// </param>
    /// <returns>
    /// Gets steering for monster in form of value from 0 to 1 of dangers in 8 directions and current intrest values
    /// </returns>
    public override (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData)
    {
        foreach(var col in aiData.Obstacles)
        {
            Vector2 dirToObs = col.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distToObs = dirToObs.magnitude;

            float weight = distToObs <= agentColliderSize ? 1 : (radius - distToObs) / radius;
            Vector2 dirNormalized = dirToObs.normalized;

            for(int i = 0; i< HelperFunctions.AllDirections.Count; i++)
            {
                float result = Vector2.Dot(dirNormalized, HelperFunctions.AllDirections[i]);

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
                Gizmos.DrawRay((Vector2)transform.position, HelperFunctions.AllDirections[i] * dangersResultTemp[i]);
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
