using UnderBounders;
using UnityEngine;

namespace MonsterScripts.WaterElemental
{
    /// <summary>
    /// Projectile creation, movement and destruction controller
    /// </summary>
    public class ProjectileController : MonoBehaviour
    {
        /// <summary>
        /// Projectile animation controller
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// Target position
        /// </summary>
        public Vector3 Target { get; set; }
        private void FixedUpdate()
        {
            if (GameStateController.Instance.isPaused)
                return;
            var currState = animator.GetCurrentAnimatorStateInfo(0);
            if (!currState.IsName("projectile_spawn") && !currState.IsName("projectile_despawn"))
                transform.position = Vector3.MoveTowards(transform.position, Target, .01f);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (GameStateController.Instance.isPaused)
                return;
            if (col.CompareTag("Detector"))
                Despwan();

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameStateController.Instance.isPaused)
                return;
            if (col.CompareTag("PlayerHitbox") || col.CompareTag("Walls"))
                Despwan();
        }

        /// <summary>
        /// Destroys projectile with appropriate animation
        /// </summary>
        private void Despwan()
        {
            animator.Play("projectile_despawn");
            Destroy(this.gameObject, .30f);
        }
    }
}
