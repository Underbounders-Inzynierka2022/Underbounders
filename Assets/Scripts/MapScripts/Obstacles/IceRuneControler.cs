using UnityEngine;

/// <summary>
/// Ice rune behaviour controller
/// </summary>
public class IceRuneControler : MonoBehaviour
{
    /// <summary>
    /// Player statistics
    /// </summary>
    [SerializeField] private PlayerSO player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.speed = player.speed / 2;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.speed = player.speed * 2;
        }
    }
}
