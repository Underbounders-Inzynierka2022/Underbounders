using UnityEngine;

namespace MonstersScripts.SlimesAI
{
    /// <summary>
    /// Abstraction for monster steering
    /// </summary>
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Gets steering for monster
        /// </summary>
        /// <param name="danger">
        /// Current dangers values
        /// </param>
        /// <param name="intrest">
        /// Current intrest values
        /// </param>
        /// <param name="aiData">
        /// Current data for steering with positions of obstacles and targets
        /// </param>
        /// <returns>
        /// Gets steering for monster in form of value from 0 to 1 of intrest in 8 directions and same number of values of danger in tuple
        /// </returns>
        public abstract (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData);
    }
}
