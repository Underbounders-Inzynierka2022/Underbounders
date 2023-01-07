using BarsElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Controller for damage dealt to the player
/// </summary>
public class PlayerDamage : MonoBehaviour, IDammageable
{
    /// <summary>
    /// Is player able to be targetted by the monsters
    /// </summary>
    public bool Targetable { get; set; }

    /// <summary>
    /// Player statistics
    /// </summary>
    [SerializeField] private PlayerSO playerVars;
    /// <summary>
    /// UI containing status bars
    /// </summary>
    [SerializeField] private UIDocument ui;
    /// <summary>
    /// Duration of the flash in seconds
    /// </summary>
    [SerializeField] private float duration = 0.025f;
    /// <summary>
    /// Material to substitute at the time of flash
    /// </summary>
    [SerializeField] private Material flashMaterial;
    /// <summary>
    /// Sprite renderer to apply flash to
    /// </summary>
    [SerializeField] private SpriteRenderer renderer;

    /// <summary>
    /// Players rigidbody
    /// </summary>
    private Rigidbody2D _rigidbody;
    /// <summary>
    /// Objects that are in player vicinity
    /// </summary>
    private List<RaycastHit2D> _collisions = new List<RaycastHit2D>();
    /// <summary>
    /// Player sprite default material
    /// </summary>
    private Material _spriteMaterial;
    /// <summary>
    /// Determines if flash coroutine has finished
    /// </summary>
    private bool _isFinished = true;
    /// <summary>
    /// Health status bar element
    /// </summary>
    private Bar _healthBar;

    void Start()
    {
        Targetable = true;
        var root = ui.rootVisualElement;
        _healthBar = root.Q<Bar>("HealthBar");
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteMaterial = renderer.material;
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

    /// <summary>
    /// Deals 1 point of damage on the player and if it is possible applies knockback
    /// </summary>
    /// <param name="damage">
    /// Damage used to implement inteface, unused due to how player damage is constructed
    /// </param>
    /// <param name="knockback">
    /// Knockback to the player
    /// </param>
    public void OnHit(float damage, Vector2 knockback)
    {
        if(playerVars.CurrentHealth > 0) playerVars.CurrentHealth -= 1f;
        TryMove(knockback);
        _healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
        Flash();
        if (playerVars.CurrentHealth == 0) GameStateController.instance.OnGameEnd();
    }

    /// <summary>
    /// Deals 1 point of damage on the player
    /// </summary>
    /// <param name="damage">
    /// Damage used to implement inteface, unused due to how player damage is constructed
    /// </param>
    public void OnHit(float damage)
    {
        if (playerVars.CurrentHealth > 0)  playerVars.CurrentHealth -= 1f;
        _healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
        Flash();
        if (playerVars.CurrentHealth == 0) GameStateController.instance.OnGameEnd();
    }

    /// <summary>
    /// Moves player back if it's possible
    /// </summary>
    /// <param name="direction">
    /// Knockback vector
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the player can be moved back moving him, <see langword="false"/> if the knockback was imposible
    /// </returns>
    public bool TryMove(Vector2 direction)
    {
        int countOfColisions = _rigidbody.Cast(direction, _collisions, Vector2.Distance(transform.position, direction));
        if (countOfColisions == 0 + _collisions.Count(c => c.collider.isTrigger))
        {

            transform.position += new Vector3(direction.x, direction.y);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Flashing controlling function
    /// </summary>
    private void Flash()
    {
        if (!_isFinished)
        {
            StopCoroutine(FlashCorutine());
        }

        StartCoroutine(FlashCorutine());
    }

    /// <summary>
    /// Coroutine responsible for player flashing on damage taken
    /// </summary>
    /// <returns>
    /// Coroutines enumerator
    /// </returns>
    private IEnumerator FlashCorutine()
    {
        _isFinished = false;
        renderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        renderer.material = _spriteMaterial;
        _isFinished = true;
        yield return null;
    }
}
