using UnityEngine;

/// <summary>
/// Projectile creation, movement and destruction controller
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Vector3 Target { get; set; }
    private void FixedUpdate()
    {
        if (GameStateController.instance.isPaused)
            return;
        var currState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currState.IsName("projectile_spawn")&&!currState.IsName("projectile_despawn"))
            transform.position = Vector3.MoveTowards(transform.position, Target, .01f);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
            return;
        if (col.CompareTag("Detector"))
            Despwan();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
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
