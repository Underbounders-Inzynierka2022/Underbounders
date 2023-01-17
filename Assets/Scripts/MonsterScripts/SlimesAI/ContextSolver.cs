using System.Collections.Generic;
using UnderBounders;
using UnityEngine;

namespace MonstersScripts.SlimesAI
{
    /// <summary>
    /// Calculates movement using steering directions
    /// </summary>
    public class ContextSolver : MonoBehaviour
    {
        /// <summary>
        /// Calculated movement vector
        /// </summary>
        private Vector2 _resultDirection = Vector2.zero;

        /// <summary>
        /// Moves slime in desired direction
        /// </summary>
        /// <param name="behaviours">
        /// List of monster steering behaviours
        /// </param>
        /// <param name="aiData">
        /// AI data containing obstacles and targets
        /// </param>
        /// <returns>
        /// Calculated movement direction
        /// </returns>
        public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
        {
            float[] danger = new float[8];
            float[] interest = new float[8];

            foreach (var behaviour in behaviours)
            {
                (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
            }

            for (int i = 0; i < 8; i++)
            {
                interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
            }

            Vector2 outputDir = Vector2.zero;
            for (int i = 0; i < 8; i++)
            {
                outputDir += HelperFunctions.AllDirections[i] * interest[i];
            }
            outputDir.Normalize();

            _resultDirection = outputDir;
            return _resultDirection;

        }

    }
}
