using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Evaporation bombs controller
/// </summary>
public class SecondaryAttack : MonoBehaviour
{
    [SerializeField] private int flashCount;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float radius;
    [SerializeField] private float damage;

    private Material spriteMaterial;

    // Start is called before the first frame update
    void Start()
    {
        spriteMaterial = renderer.material;
        StartCoroutine(Detonate());
    }

    /// <summary>
    /// Flashing coroutine handling bomb lifespan with flashing
    /// </summary>
    /// <returns>
    /// Coroutines enumerator
    /// </returns>
    private IEnumerator Detonate()
    {
        for(int i = 0; i<flashCount; i++)
        {
            renderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            renderer.material = spriteMaterial;
            yield return new WaitForSeconds(flashDuration);
        }
        renderer.material = flashMaterial;
        DealDamage();
        yield return new WaitForSeconds(0.02f);
        Destroy(gameObject, 0.02f);
    }

    /// <summary>
    /// Deals damage to monsters and player
    /// </summary>
    private void DealDamage()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(var collider in colliders)
        {
            if (collider.CompareTag("PlayerHitbox"))
            {
                var damager = collider.GetComponentInParent<PlayerDamage>();
                var dir = (Vector2)(transform.position - collider.transform.position).normalized;
                damager.OnHit(damage, dir * radius);
            }
            if(collider.CompareTag("Turret") || collider.CompareTag("MeleeEnemy"))
            {
                var damager = collider.GetComponentInParent<MonsterDamage>();
                damager.OnHit(damage);
            }
        }
    
    }



}
