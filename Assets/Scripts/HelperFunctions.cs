using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnderBounders
{
    /// <summary>
    /// Static class for common functions
    /// </summary>
    public static class HelperFunctions
    {

        /// <summary>
        /// Randomly draws one weight from list of weights
        /// </summary>
        /// <param name="weights">
        /// List of weights in order
        /// </param>
        /// <returns>
        /// Index of weight chosen
        /// </returns>
        public static int RandomWeighted(List<float> weights)
        {
            float weightTotal = weights.Sum();
            int result;
            float total = 0;
            float randVal = Random.Range(0f, weightTotal);
            for (result = 0; result < weights.Count; result++)
            {
                total += weights[result];
                if (total >= randVal) break;
            }
            return result;
        }

        /// <summary>
        /// All eight directions nomalized for behaviours to use
        /// </summary>
        public static List<Vector2> AllDirections => new List<Vector2>()
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
