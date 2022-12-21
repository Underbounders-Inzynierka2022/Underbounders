using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : PickUp
{
    public override bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return playerStatsController.AmmoPickUp();
    }
}
