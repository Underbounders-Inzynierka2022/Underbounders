using System;

/// <summary>
/// Obstacle type and its chance modifier
/// </summary>
[Serializable]
public class ObstacleDictionaryPair 
{
    /// <summary>
    /// Obstacle type
    /// </summary>
    public ObstacleType obstacle;
    /// <summary>
    /// Propability modifier for particular obstacle
    /// </summary>
    public float chance;
}
