using UnityEngine;

/// <summary>
/// Pick up behaviour for items
/// </summary>
public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            var playerStatsController = collision.GetComponentInParent<PlayerStatsController>();
            bool success = OnPickUp(playerStatsController);
            if(success) Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// Virtual method describing picking up the item
    /// </summary>
    /// <param name="playerStatsController">
    /// Player stats controller of player, that picks up item
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the item was used, <see langword="false"/> if the item wasn't used
    /// </returns>
    public virtual bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return false;
    }
}
