using CommonInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterScripts
{
    /// <summary>
    /// Damage controller for monsters
    /// </summary>
    public class MonsterDamage : MonoBehaviour, IDammageable
    {
        /// <summary>
        /// Current monster health
        /// </summary>
        public float Health { get; set; }
        /// <summary>
        /// Determines if monster is targetable
        /// </summary>
        public bool Targetable { get; set; }

        /// <summary>
        /// Maximum amount of health
        /// </summary>
        [SerializeField] private float maxHealth;
        /// <summary>
        /// Health display for debuging
        /// </summary>
        [SerializeField] private float seeHealth;
        /// <summary>
        /// Duration of the flash in seconds
        /// </summary>
        [SerializeField] private float duration = 0.025f;
        /// <summary>
        /// Material to replace during flash
        /// </summary>
        [SerializeField] private Material flashMaterial;
        /// <summary>
        /// Monster sprite renderer
        /// </summary>
        [SerializeField] private SpriteRenderer renderer;

        /// <summary>
        /// Monster rigidbody
        /// </summary>
        private Rigidbody2D _rigidbody;
        /// <summary>
        /// Colliders detected in monster vicinity
        /// </summary>
        private List<RaycastHit2D> _collisions = new List<RaycastHit2D>();
        /// <summary>
        /// Default sprite material
        /// </summary>
        private Material _spriteMaterial;
        /// <summary>
        /// Determiness if flash has finished
        /// </summary>
        private bool _isFinished = true;


        void Start()
        {
            Targetable = true;
            Health = maxHealth;
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteMaterial = renderer.material;
        }

        /// <summary>
        /// Deals damage to monster and if it is possible applies knockback
        /// </summary>
        /// <param name="damage">
        /// Damage used to implement inteface, unused due to how player damage is constructed
        /// </param>
        /// <param name="knockback">
        /// Knockback to the player
        /// </param>
        public void OnHit(float damage, Vector2 knockback)
        {
            TryMove(knockback);

            Health -= damage;
            Flash();
        }
        /// <summary>
        /// Deals damage to monster
        /// </summary>
        /// <param name="damage">
        /// Damage dealt
        /// </param>
        public void OnHit(float damage)
        {
            Health -= damage;
            Flash();
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
        /// <summary>
        /// Moves player back if it's possible
        /// </summary>
        /// <param name="direction">
        /// Knockback vector
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the monster can be moved back moving him, <see langword="false"/> if the knockback was imposible
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


    }
}
