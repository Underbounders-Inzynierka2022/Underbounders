using UnityEngine;

/// <summary>
/// Detectors abstraction layer
/// </summary>
public abstract class Detector : MonoBehaviour
{
    /// <summary>
    /// Detects targets in vasinity
    /// </summary>
    /// <param name="ai">
    /// Data holding curent targets and rest of the data 
    /// </param>
    public abstract void Detect(AIData ai);
}
