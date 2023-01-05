using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sword attacking hitbox controller
/// </summary>
public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private float posOffset;
    [SerializeField] private PlayerSO playerStats;

    private Vector3 _righAttackAngleOffset;
    private Vector3 _rightAttackPosition;

    private void Start()
    {
        _righAttackAngleOffset = transform.localEulerAngles;
        _rightAttackPosition = transform.localPosition;
    }
    /// <summary>
    /// Control attack i regards to direction
    /// </summary>
    /// <param name="dir">
    /// Direction of atacking
    /// </param>
    public void Attack(Direction dir)
    {
        BeginAttackState();
        switch (dir)
        {
            case Direction.left:
                AttackLeft();
                break;
            case Direction.right:
                AttackRight();
                break;
            case Direction.up:
                AttackUp();
                break;
            case Direction.down:
                AttackDown();
                break;
        }
    }

    /// <summary>
    /// Begins atacking state and centers hitbox
    /// </summary>
    private void BeginAttackState()
    {
        collider.enabled = true;
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Moves and rotates hitbox for left direction
    /// </summary>
    private void AttackLeft()
    {
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y-180, _righAttackAngleOffset.z);
        transform.localPosition = _rightAttackPosition;
    }

    /// <summary>
    /// Moves and rotates hitbox for right direction
    /// </summary>
    private void AttackRight()
    {
        transform.localEulerAngles = _righAttackAngleOffset;
        transform.localPosition = _rightAttackPosition;
    }

    /// <summary>
    /// Moves and rotates hitbox for upwards direction
    /// </summary>
    private void AttackUp()
    {
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y, _righAttackAngleOffset.z+90);
        transform.localPosition = new Vector3(_rightAttackPosition.x - posOffset, _rightAttackPosition.y - posOffset, _rightAttackPosition.z);
    }

    /// <summary>
    /// Moves and rotates hitbox for downwards driection
    /// </summary>
    private void AttackDown()
    {
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y, _righAttackAngleOffset.z + 270);
        transform.localPosition = new Vector3(_rightAttackPosition.x + posOffset, _rightAttackPosition.y + posOffset, _rightAttackPosition.z);
    }

    /// <summary>
    /// Finishes attacking state
    /// </summary>
    public void AttackFinish()
    {
        collider.enabled = false;
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
            return;
        var healthControllers = col.GetComponents<MonsterDamage>();
        if (healthControllers.Length > 0)
        {
            foreach(var healthController in healthControllers)
            {
                if (col.CompareTag("Turret"))
                {
                    healthController.OnHit(playerStats.attack);
                }
                else if (col.CompareTag("MeleeEnemy"))
                {
                    Vector2 direction = (col.transform.position - transform.parent.transform.position).normalized;
                    healthController.OnHit(playerStats.attack, direction * (playerStats.knocbackMultiplier / 6f));
                }
            }
        }
    }
}
