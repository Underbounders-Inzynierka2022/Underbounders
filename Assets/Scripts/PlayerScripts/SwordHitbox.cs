using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    private Vector3 righAttackOffset;

    private void Start()
    {
        righAttackOffset = transform.localEulerAngles;
    }

    public void Attack(Direction dir)
    {
        switch (dir)
        {
            case Direction.left:
                AttackLeft();
                break;
            case Direction.right:
                AttackRight();
                break;
        }
    }

    private void AttackLeft()
    {
        _collider.enabled = true;
        transform.localEulerAngles = new Vector3(righAttackOffset.x, righAttackOffset.y-180, righAttackOffset.z);
    }

    private void AttackRight()
    {
        _collider.enabled = true;
        Debug.Log(_collider.enabled);
        transform.localEulerAngles = righAttackOffset;
    }

    public void AttackFinish()
    {
        _collider.enabled = false;
    }
}
