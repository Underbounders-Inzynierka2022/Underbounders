using UnderBounders;
using UnityEngine;


namespace MonsterScripts.WaterElemental
{
    /// <summary>
    /// Controls water elemental behaviours
    /// </summary>
    public class WaterElementalController : MonoBehaviour
    {
        /// <summary>
        /// Direction monster is facing
        /// </summary>
        [SerializeField] private Direction direction = Direction.right;
        /// <summary>
        /// Water elemental animation controller
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// Pojectile spawned by water elemental
        /// </summary>
        [SerializeField] private GameObject projectile;
        /// <summary>
        /// Time between projectile spawning
        /// </summary>
        [SerializeField] private float timeToSpawn;
        /// <summary>
        /// Monster damage controller
        /// </summary>
        [SerializeField] private MonsterDamage damage;

        /// <summary>
        /// Detected player
        /// </summary>
        private GameObject _player;
        /// <summary>
        /// Determines if player was detected
        /// </summary>
        private bool _detected;
        /// <summary>
        /// Time till next projectile spawn
        /// </summary>
        private float _currTimeToSpawn;

        void Start()
        {
            animator.SetInteger("Direction", 0);
            animator.Play("WaterElemental_hidden");
            _currTimeToSpawn = timeToSpawn;
        }

        private void FixedUpdate()
        {
            if (GameStateController.Instance.isPaused)
                return;
            if (damage.Health <= 0)
            {
                Despawn();
                Destroy(transform.parent.gameObject, .4f);
                _detected = false;
            }
            if (_detected)
            {
                if (_currTimeToSpawn <= 0)
                {
                    var dir = _player.transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90;
                    var instance = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.Euler(0, 0, -angle));
                    RotateWaterElemental(dir);
                    instance.GetComponent<ProjectileController>().Target = _player.transform.position + dir.normalized * 5f;
                    _currTimeToSpawn = timeToSpawn;
                }
                else
                {
                    _currTimeToSpawn -= 1f;
                }
            }

        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (GameStateController.Instance.isPaused)
                return;
            if (col.CompareTag("Player"))
            {
                Spawn();
                _detected = true;
                _player = col.gameObject;
            }

        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.CompareTag("Player") && !GameStateController.Instance.isPaused && !_detected)
            {
                Spawn();
                _detected = true;
                _player = col.gameObject;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (GameStateController.Instance.isPaused)
                return;
            if (col.CompareTag("Player"))
            {
                Despawn();
                _detected = false;
            }

        }

        /// <summary>
        /// Plays spawn animation
        /// </summary>
        public void Spawn()
        {
            animator.Play("WaterElemental_spawn");
        }

        /// <summary>
        /// Plays despawn animation
        /// </summary>
        public void Despawn()
        {
            animator.Play("WaterElemental_despawn");
        }
        /// <summary>
        /// Plays water elemental animation directed towards player
        /// </summary>
        /// <param name="dir">
        /// Direction of rotation towards to
        /// </param>
        private void RotateWaterElemental(Vector2 dir)
        {
            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    animator.Play("WatterElemental_idle_right");
                    animator.SetInteger("Direction", 0);
                }
                else
                {
                    animator.Play("WatterElemental_idle_left");
                    animator.SetInteger("Direction", 1);
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    animator.Play("WatterElemental_idle_up");
                    animator.SetInteger("Direction", 2);
                }
                else
                {
                    animator.Play("WatterElemental_idle_down");
                    animator.SetInteger("Direction", 3);
                }
            }
        }

    }
}
