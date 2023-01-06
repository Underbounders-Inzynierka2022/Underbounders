/// <summary>
/// Pick up healing item behaviour
/// </summary>
public class HealthPickUp : PickUp
{
    /// <summary>
    /// Picks up healing item if it player can pick it up
    /// </summary>
    /// <param name="playerStatsController">
    /// Player stats controller of player, that picks up item
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the item was used, <see langword="false"/> if the item wasn't used
    /// </returns>
    public override bool OnPickUp(PlayerStatsController playerStatsController)
    {
        return playerStatsController.Heal();
    }
}
