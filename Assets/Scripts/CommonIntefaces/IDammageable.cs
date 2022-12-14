using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDammageable
{
    public bool Targetable { set; get; }
    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);
}
