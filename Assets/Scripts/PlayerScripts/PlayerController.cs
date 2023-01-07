using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Controls movement and on map input of the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Player statistics
    /// </summary>
    [SerializeField] private PlayerSO player;
    /// <summary>
    /// Contact filtering object
    /// </summary>
    [SerializeField] private ContactFilter2D moveFilter;
    /// <summary>
    /// Offset helping to check if player can be moved
    /// </summary>
    [SerializeField] private float collisionOffSet = 0.05f;
    /// <summary>
    /// Sword hitbox controller
    /// </summary>
    [SerializeField] private SwordHitbox sword;
    /// <summary>
    /// Bomb object to spawn
    /// </summary>
    [SerializeField] private GameObject secondary;
    /// <summary>
    /// Player stats controller
    /// </summary>
    [SerializeField] private PlayerStatsController statsController;

    /// <summary>
    /// Players rigidbody
    /// </summary>
    private Rigidbody2D _rigidbody;
    /// <summary>
    /// Player animation controller
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Movement input form keyboard or controller
    /// </summary>
    private Vector2 _movementInput;
    /// <summary>
    /// Direction player is facing
    /// </summary>
    private Direction _direction = Direction.right;
    /// <summary>
    /// Colliders in player vicinity
    /// </summary>
    private List<RaycastHit2D> _collisions = new List<RaycastHit2D>();
    /// <summary>
    /// Determines if player is in attacking state
    /// </summary>
    private bool _attack = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

    }


    private void FixedUpdate()
    {
        if (GameStateController.instance.isPaused)
        {
            _movementInput = Vector2.zero;
            return;
        }

        if (_attack)
        {
            sword.Attack(_direction);
            return;
        }
        sword.AttackFinish();

        if (_movementInput != Vector2.zero)
        {
            if (TryMove(_movementInput))
                PlayAnimationInDirection("player_go_");  
        }
        else 
        {
            PlayAnimationInDirection("player_idle_");
        }

    }

    /// <summary>
    /// Divides movement and  saves movement from input
    /// </summary>
    /// <param name="movementValue">
    /// Movement input 
    /// </param>
    private void OnMove(InputValue movementValue)
    {
        if (GameStateController.instance.isPaused)
        {
            _movementInput = Vector2.zero;
            return;
        }
            
        _movementInput = movementValue.Get<Vector2>();
        if(Math.Abs(_movementInput.x)>= Math.Abs(_movementInput.y))
        {
            if (_movementInput.x < 0)
            {
                _direction = Direction.left;
            }
            else if (_movementInput.x > 0)
            {
                _direction = Direction.right;
            }
        } 
        else if(Math.Abs(_movementInput.x) < Math.Abs(_movementInput.y))
        {
            if (_movementInput.y < 0)
            {
                _direction = Direction.down;
            }
            else if (_movementInput.y > 0)
            {
                _direction = Direction.up;
            }
        }

    }
    /// <summary>
    /// Makes player attack
    /// </summary>
    private void OnFire()
    {
        if (GameStateController.instance.isPaused || _attack)
            return;
        _attack = true;
        PlayAnimationInDirection("player_sword_");
        Invoke("EndAttack", (float)(0.48 - player.attackSpeed / 10));
    }
    /// <summary>
    /// Uses secondary attack
    /// </summary>
    private void OnSecondary()
    {
        if (GameStateController.instance.isPaused)
            return;
        if (statsController.UseSecondary())
            Instantiate(secondary, transform.position, transform.rotation);
    }
    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
    private void OnPause()
    {
        if (GameStateController.instance.isPaused)
        {
            GameStateController.instance.UnloadPauseMenu();
        }
        else
        {
            GameStateController.instance.LoadPauseMenu();
        }
    }

    /// <summary>
    /// Moves player if possible 
    /// </summary>
    /// <param name="Direction">
    /// Nonnormalized direction of movement
    /// </param>
    /// <returns></returns>
    private bool TryMove(Vector2 Direction)
    {
        int countOfColisions = _rigidbody.Cast(Direction, moveFilter, _collisions, player.speed * Time.fixedDeltaTime + collisionOffSet);
        if (countOfColisions == 0 || _collisions.Any(c => c.collider.isTrigger))
        {
            _rigidbody.MovePosition(_rigidbody.position + Direction * player.speed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }


    /// <summary>
    /// Ends attack state
    /// </summary>
    private void EndAttack()
    {
        _attack = false; 
    }

    /// <summary>
    /// Plays animation from set in particular direction
    /// </summary>
    /// <param name="animationName">
    /// Name of animation set ended with _
    /// </param>
    private void PlayAnimationInDirection(string animationName)
    {
        _animator.Play($"{animationName}{_direction.ToString().ToLower()}");
    }
}
