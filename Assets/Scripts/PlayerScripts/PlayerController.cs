using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementInput;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    private Direction _direction = Direction.right;
    private bool _attack = false;
    [SerializeField] private PlayerSO player;
    [SerializeField] private ContactFilter2D _moveFilter;
    [SerializeField] private float _collisionOffSet = 0.05f;
    [SerializeField] private SwordHitbox _sword;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        if (_attack)
        {
            _sword.Attack(_direction);
            return;
        }
        _sword.AttackFinish();

        if (movementInput != Vector2.zero)
        {
            if (TryMove(movementInput))
            {
                switch (_direction)
                {
                    case Direction.left:
                        animator.Play("player_go_left");
                        break;
                    case Direction.right:
                        animator.Play("player_go_right");
                        break;
                    case Direction.up:
                        animator.Play("player_go_up");
                        break;
                    case Direction.down:
                        animator.Play("player_go_down");
                        break;
                }
            }
        }
        else 
        {
            switch (_direction)
            {
                case Direction.left:
                    animator.Play("player_idle_left");
                    break;
                case Direction.right:
                    animator.Play("player_idle_right");
                    break;
                case Direction.up:
                    animator.Play("player_idle_up");
                    break;
                case Direction.down:
                    animator.Play("player_idle_down");
                    break;
            }
        }

    }

    private bool TryMove(Vector2 Direction)
    {
        int countOfColisions = rigidbody.Cast(movementInput, _moveFilter, collisions, player.speed * Time.fixedDeltaTime + _collisionOffSet);
        if (countOfColisions == 0 || collisions.Any(c=> c.collider.isTrigger))
        {
            rigidbody.MovePosition(rigidbody.position + movementInput * player.speed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
        if(Math.Abs(movementInput.x)>= Math.Abs(movementInput.y))
        {
            if (movementInput.x < 0)
            {
                _direction = Direction.left;
            }
            else if (movementInput.x > 0)
            {
                _direction = Direction.right;
            }
        } 
        else if(Math.Abs(movementInput.x) < Math.Abs(movementInput.y))
        {
            if (movementInput.y < 0)
            {
                _direction = Direction.down;
            }
            else if (movementInput.y > 0)
            {
                _direction = Direction.up;
            }
        }

    }

    void OnFire()
    {
        _attack = true;
        switch (_direction)
        {
            case Direction.left:
                animator.Play("player_sword_left");
                break;
            case Direction.right:
                animator.Play("player_sword_right");
                break;
            case Direction.up:
                animator.Play("player_sword_up");
                break;
            case Direction.down:
                animator.Play("player_sword_down");
                break;
        }
        Invoke("SetAttack", 0.34f);
        
    }

    void SetAttack()
    {
        _attack = !_attack; 
    }
}
