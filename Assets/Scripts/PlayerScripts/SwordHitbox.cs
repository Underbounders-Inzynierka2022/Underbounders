using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _posOffset;

    private Vector3 _righAttackAngleOffset;

    private Vector3 _rightAttackPosition;

    private void Start()
    {
        _righAttackAngleOffset = transform.localEulerAngles;
        _rightAttackPosition = transform.localPosition;
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
            case Direction.up:
                AttackUp();
                break;
            case Direction.down:
                AttackDown();
                break;
        }
    }

    private void AttackLeft()
    {
        _collider.enabled = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y-180, _righAttackAngleOffset.z);
        transform.localPosition = _rightAttackPosition;
    }

    private void AttackRight()
    {
        _collider.enabled = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = _righAttackAngleOffset;
        transform.localPosition = _rightAttackPosition;
    }

    private void AttackUp()
    {
        _collider.enabled = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y, _righAttackAngleOffset.z+90);
        transform.localPosition = new Vector3(_rightAttackPosition.x - _posOffset, _rightAttackPosition.y - _posOffset, _rightAttackPosition.z);
    }

    private void AttackDown()
    {
        _collider.enabled = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(_righAttackAngleOffset.x, _righAttackAngleOffset.y, _righAttackAngleOffset.z + 270);
        transform.localPosition = new Vector3(_rightAttackPosition.x + _posOffset, _rightAttackPosition.y + _posOffset, _rightAttackPosition.z);
    }

    public void AttackFinish()
    {
        _collider.enabled = false;
        
    }
}
