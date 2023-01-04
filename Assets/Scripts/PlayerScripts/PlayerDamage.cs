using BarsElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDamage : MonoBehaviour, IDammageable
{
    [SerializeField] private PlayerSO playerVars;

    [SerializeField] private UIDocument ui;
    [SerializeField] private float duration = 0.025f;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    private Material spriteMaterial;
    private bool isFinished = true;

    public bool Targetable { get; set; }

    private Bar healthBar;

    public void OnHit(float damage, Vector2 knockback)
    {
        if(playerVars.CurrentHealth > 0) playerVars.CurrentHealth -= 1f;
        TryMove(knockback);
        healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
        Flash();
        if (playerVars.CurrentHealth == 0) GameStateController.instance.OnGameEnd();
    }

    public bool TryMove(Vector2 direction)
    {
        int countOfColisions = rigidbody.Cast(direction, collisions,Vector2.Distance(transform.position, direction));
        if (countOfColisions == 0 + collisions.Count(c => c.collider.isTrigger))
        {
            
            transform.position += new Vector3(direction.x, direction.y);
            return true;
        }

        return false;
    }

    public void OnHit(float damage)
    {
        if (playerVars.CurrentHealth > 0)  playerVars.CurrentHealth -= 1f;
        healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
        Flash();
        if (playerVars.CurrentHealth == 0) GameStateController.instance.OnGameEnd();
    }
    void Start()
    {
        Targetable = true;
        var root = ui.rootVisualElement;
        healthBar = root.Q<Bar>("HealthBar");
        rigidbody = GetComponent<Rigidbody2D>();
        spriteMaterial = renderer.material;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
            return;
        if (col.CompareTag("FireRune"))
        {
            OnHit(0f);
        }
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
}
