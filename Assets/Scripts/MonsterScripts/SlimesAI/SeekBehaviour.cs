using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Behaviour for seeking the players
/// </summary>
public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private float threashold = 0.5f;

    [SerializeField] private bool showGizmo = true;


    bool _reachedLastTarget = true;
    private Vector2 _targetPositionCache;
    float[] _interestResultTemp = null;

    /// <summary>
    /// Applies intrest depending on current target position
    /// </summary>
    /// <param name="danger">
    /// Current dangers values
    /// </param>
    /// <param name="intrest">
    /// Previous intrest values
    /// </param>
    /// <param name="aiData">
    /// Current data for steering with positions of obstacles and targets
    /// </param>
    /// <returns>
    /// Gets steering for monster in form of value from 0 to 1 of intrest in 8 directions and current dangers values
    /// </returns>
    public override (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData)
    {
        if (_reachedLastTarget)
        {
            if(aiData.Targets is null || aiData.Targets.Count <= 0)
            {
                aiData.CurrentTarget = null;
                return (danger, intrest);
            }
            _reachedLastTarget = false;
            aiData.CurrentTarget = aiData.Targets.OrderBy(t => Vector2.Distance(t.position, transform.position)).FirstOrDefault();
        }

        if (aiData.CurrentTarget != null && aiData.Targets != null && aiData.Targets.Contains(aiData.CurrentTarget))
            _targetPositionCache = aiData.CurrentTarget.position;

        if(Vector2.Distance(transform.position, _targetPositionCache) < 0.1f)
        {
            _reachedLastTarget = true;
            aiData.CurrentTarget = null;
            return (danger, intrest);
        }

        Vector2 dirToTarget = (_targetPositionCache - (Vector2)transform.position);
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
        _interestResultTemp = intrest;
        return (danger, intrest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        Gizmos.DrawWireSphere(_targetPositionCache, 0.1f);

        if (Application.isPlaying && _interestResultTemp != null)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < _interestResultTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * _interestResultTemp[i]);
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
