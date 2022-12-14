using BarsElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDamage : MonoBehaviour, IDammageable
{
    [SerializeField] private PlayerSO playerVars;

    [SerializeField] private UIDocument ui;

    public bool Targetable { get; set; }

    private Bar healthBar;

    public void OnHit(float damage, Vector2 knockback)
    {
        playerVars.CurrentHealth -= 1f;
        transform.position = new Vector3(transform.position.x + knockback.x, transform.position.y + knockback.y, transform.position.z);
        healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
    }

    public void OnHit(float damage)
    {
        playerVars.CurrentHealth -= 1f;
        healthBar.value = (int)Mathf.Ceil(playerVars.CurrentHealth);
    }
    void Start()
    {
        Targetable = true;
        var root = ui.rootVisualElement;
        healthBar = root.Q<Bar>("HealthBar");
        healthBar.value = (int) Mathf.Ceil(playerVars.CurrentHealth);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FireRune"))
        {
            OnHit(0f);
        }
    }
}
