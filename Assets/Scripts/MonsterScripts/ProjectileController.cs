using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Vector3 Target { get; set; }
    private void FixedUpdate()
    {
        //if (GameStateController.instance.isPaused)
            //return;
        var currState = _animator.GetCurrentAnimatorStateInfo(0);
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
        if (col.CompareTag("PlayerHitbox"))
            Despwan();
    }

    private void Despwan()
    {
        _animator.Play("projectile_despawn");
        Destroy(this.gameObject, .30f);
    }
}
