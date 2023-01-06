using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : PickUp
{
    /// <summary>
    /// Picks up ammunition if it player can pick it up
    /// </summary>
    /// <param name="playerStatsController">
    /// Player stats controller of player, that picks up item
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the item was used, <see langword="false"/> if the item wasn't used
    /// </returns>
    public override bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return playerStatsController.AmmoPickUp();
    }
}
