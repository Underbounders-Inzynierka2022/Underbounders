using UnderBounders;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Checks if player was hit
    /// </summary>
    public class EnemyHitChecker : MonoBehaviour
    {
        /// <summary>
        /// Player damage controller
        /// </summary>
        [SerializeField] private PlayerDamage playerDamage;
        /// <summary>
        /// Sword collider, that gives protection
        /// </summary>
        [SerializeField] private Collider2D swordCollider;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameStateController.Instance.isPaused || swordCollider.enabled)
                return;
            if (col.CompareTag("Projectile"))
            {
                Vector2 knockback = (transform.position - col.gameObject.transform.position).normalized;
                playerDamage.OnHit(0f, knockback * 0.1f);
            }
            if (col.CompareTag("MeleeEnemy"))
            {
                Vector2 knockback = (transform.position - col.gameObject.transform.position).normalized;
                playerDamage.OnHit(0f, knockback * 0.1f);
            }
        }
    }
}
