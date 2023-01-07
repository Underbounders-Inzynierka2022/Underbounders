using System.Collections;
using UnityEngine;

/// <summary>
/// Evaporation bombs controller
/// </summary>
public class SecondaryAttack : MonoBehaviour
{
    /// <summary>
    /// Number of flashes till explosion
    /// </summary>
    [SerializeField] private int flashCount;
    /// <summary>
    /// Duration of the flash in seconds
    /// </summary>
    [SerializeField] private float flashDuration;
    /// <summary>
    /// Material to substitute at the time of flash
    /// </summary>
    [SerializeField] private Material flashMaterial;
    /// <summary>
    /// Sprite renderer to apply flash to
    /// </summary>
    [SerializeField] private SpriteRenderer renderer;
    /// <summary>
    /// Radius of damage infliction
    /// </summary>
    [SerializeField] private float radius;
    /// <summary>
    /// Damage that bomb inflicts on monsters
    /// </summary>
    [SerializeField] private float damage;

    /// <summary>
    /// Default sprite material of the bomb
    /// </summary>
    private Material spriteMaterial;


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
