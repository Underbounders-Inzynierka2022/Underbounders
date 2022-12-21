using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterDamage : MonoBehaviour, IDammageable
{
    public float Health{get; set;}
    [SerializeField] private float maxHealth;
    [SerializeField] private float seeHealth;
    [SerializeField] private float duration = 0.025f;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    private Material spriteMaterial;
    private bool isFinished = true;

    public bool Targetable { get; set; }

    public void OnHit(float damage, Vector2 knockback)
    {
        TryMove(knockback);
        
        Health -= damage;
        Flash();
    }

    private void Flash()
    {
        if (!isFinished)
        {
            StopCoroutine(FlashCorutine());
        }

        StartCoroutine(FlashCorutine());
    }

    private IEnumerator FlashCorutine()
    {
        isFinished = false;
        renderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        renderer.material = spriteMaterial;
        isFinished = true;
        yield return null;
    }

    public bool TryMove(Vector2 direction)
    {
        int countOfColisions = rigidbody.Cast(direction, collisions, Vector2.Distance(transform.position, direction));
        if (countOfColisions == 0 + collisions.Count(c => c.collider.isTrigger))
        {

            transform.position += new Vector3(direction.x, direction.y);
            return true;
        }

        return false;
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        Flash();
    }

    // Start is called before the first frame update
    void Start()
    {
        Targetable = true;
        Health = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteMaterial = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
