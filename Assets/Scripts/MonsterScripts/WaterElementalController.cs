using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterElementalController : MonoBehaviour
{
    [SerializeField] private Direction _direction = Direction.right;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private float _timeToSpawn;

    [SerializeField] private MonsterDamage damage;

    private GameObject _player;
    private bool _detected;
    private float _currTimeToSpawn;

    void Start()
    {
        _animator.SetInteger("Direction", 0);
        _animator.Play("WaterElemental_hidden");
        _currTimeToSpawn = _timeToSpawn;
    }

    private void FixedUpdate()
    {
        if (GameStateController.instance.isPaused)
            return;
        if (damage.Health <= 0)
        {
            Despawn();
            Destroy(transform.parent.gameObject, .4f);
            _detected = false;
        }
        if (_detected)
        {
            if(_currTimeToSpawn <= 0)
            {
                var dir = _player.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90;
                var instance = Instantiate(_projectile, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.Euler(0, 0, -angle));
                RotateWaterElemental(dir);
                instance.GetComponent<ProjectileController>().Target = _player.transform.position + dir.normalized * 2f;
                _currTimeToSpawn = _timeToSpawn;
            }
            else
            {
                _currTimeToSpawn -= 1f;
            }
        }

    }

    public void Spawn()
    {
        _animator.Play("WaterElemental_spawn");
    }

    public void Despawn()
    {
        _animator.Play("WaterElemental_despawn");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
            return;
        if (col.CompareTag("Player"))
        {
            Spawn();
            _detected = true;
            _player = col.gameObject;
        }
            
    }

     void OnTriggerExit2D(Collider2D col)
    {
        if (GameStateController.instance.isPaused)
            return;
        if (col.CompareTag("Player"))
        {
            Despawn();
            _detected = false;
        }
            
    }

    private void RotateWaterElemental(Vector2 dir)
    {
        if(Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            if(dir.x > 0)
            {
                _animator.Play("WatterElemental_idle_right");
                _animator.SetInteger("Direction", 0);
            }
            else
            {
                _animator.Play("WatterElemental_idle_left");
                _animator.SetInteger("Direction", 1);
            }
        }
        else
        {
            if (dir.y > 0)
            {
                _animator.Play("WatterElemental_idle_up");
                _animator.SetInteger("Direction", 2);
            }
            else
            {
                _animator.Play("WatterElemental_idle_down");
                _animator.SetInteger("Direction", 3);
            }
        }
        
        
    }

}
