using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp
{
    public override bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return playerStatsController.Heal();
    }
}
