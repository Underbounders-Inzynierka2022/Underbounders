using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage handling interface
/// </summary>
public interface IDammageable
{
    public bool Targetable { set; get; }
    /// <summary>
    /// Deals certain amount of damage with knockback
    /// </summary>
    /// <param name="damage">
    /// Damamge dealt
    /// </param>
    /// <param name="knockback">
    /// Knockback to be added to position
    /// </param>
    public void OnHit(float damage, Vector2 knockback);
    /// <summary>
    /// Deals certain amount of damage
    /// </summary>
    /// <param name="damage">
    /// Damamge dealt
    /// </param>
    public void OnHit(float damage);
}
