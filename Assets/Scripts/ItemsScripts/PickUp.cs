using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public virtual bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return false;
    }
}
