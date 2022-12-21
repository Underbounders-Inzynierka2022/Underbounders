using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitChecker : MonoBehaviour
{
    [SerializeField] private PlayerDamage _playerDamage;
    [SerializeField] private Collider2D _swordCollider;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused || _swordCollider.enabled)
            return;
        if (col.CompareTag("Projectile"))
        {
            Vector2 knockback = (transform.position - col.gameObject.transform.position).normalized;
            _playerDamage.OnHit(0f, knockback * 0.1f);
        }
        if (col.CompareTag("MeleeEnemy"))
        {
            Vector2 knockback = (transform.position - col.gameObject.transform.position).normalized;
            _playerDamage.OnHit(0f, knockback * 0.1f);
        }
    }
}
