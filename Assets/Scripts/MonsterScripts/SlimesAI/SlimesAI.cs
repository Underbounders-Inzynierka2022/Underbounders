using MonsterScripts;
using System.Collections;
using System.Collections.Generic;
using UnderBounders;
using UnityEngine;

namespace MonstersScripts.SlimesAI
{
    /// <summary>
    /// Class controling all behaviours of slimes
    /// </summary>
    public class SlimesAI : MonoBehaviour
    {
        /// <summary>
        /// Steering behaiours of slimes
        /// </summary>
        [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
        /// <summary>
        /// Obstacles and targets detectors 
        /// </summary>
        [SerializeField] private List<Detector> detectors;
        /// <summary>
        /// Decision controller for steering
        /// </summary>
        [SerializeField] private ContextSolver contextSolver;
        /// <summary>
        /// Data containing targets and obstacles
        /// </summary>
        [SerializeField] private AIData aiData;
        /// <summary>
        /// Delay provided for optimalization
        /// </summary>
        [SerializeField] private float delay = 0.05f, aiUpdateDelay = 0.9f;
        /// <summary>
        /// Kind of slime for animation controller
        /// </summary>
        [SerializeField] private string slimeKind;
        /// <summary>
        /// Slime animation controller
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// Speed of slime
        /// </summary>
        [SerializeField] private float speed;
        /// <summary>
        /// Monster damage controller
        /// </summary>
        [SerializeField] private MonsterDamage damage;

        /// <summary>
        /// Direction of slime facing
        /// </summary>
        private Direction _direction;
        /// <summary>
        /// Determines if slime is following target
        /// </summary>
        private bool _follow = false;

        void Start()
        {
            InvokeRepeating("PerformDetection", 0, delay);
            _direction = Direction.left;
        }

        void Update()
        {
            if (GameStateController.Instance.isPaused)
            {
                StopAllCoroutines();
                aiData.CurrentTarget = null;
                _follow = false;
                StartCoroutine(Wait());
                return;
            }

            if (damage.Health <= 0)
            {
                Destroy(this.gameObject);
            }
            if (aiData.CurrentTarget != null)
            {
                if (!_follow)
                {
                    _follow = !_follow;
                    StartCoroutine(Chase());
                }
            }
            else if (aiData.GetTargetsCount() > 0)
            {
                aiData.CurrentTarget = aiData.Targets[0];
            }
            else
            {
                PlayIdle();
            }
        }
        /// <summary>
        /// Performs detecting by all of the detectors
        /// </summary>
        private void PerformDetection()
        {
            foreach (var detector in detectors)
            {
                detector.Detect(aiData);
            }
        }
        /// <summary>
        /// Plays slime idle animation for particular slime kind and direction
        /// </summary>
        private void PlayIdle()
        {
            string animationName = "_idle_";
            animator.Play($"{slimeKind}{animationName}{_direction.ToString()}");

        }
        /// <summary>
        /// Moves slime towards target
        /// </summary>
        /// <param name="target">
        /// Target position
        /// </param>
        private void Move(Vector2 target)
        {
            Vector2 dir = -(Vector2)transform.position + target;
            transform.position = (Vector2)transform.position + target * speed;

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                if (dir.x >= 0)
                {
                    _direction = Direction.right;
                }
                else
                {
                    _direction = Direction.left;
                }
            }
            else
            {
                if (dir.y >= 0)
                {
                    _direction = Direction.up;
                }
                else
                {
                    _direction = Direction.down;
                }
            }
            PlayRun();
        }
        /// <summary>
        /// Plays running slime animation for particular slime kind and direction
        /// </summary>
        private void PlayRun()
        {
            string animationName = "_run_";
            animator.Play($"{slimeKind}{animationName}{_direction.ToString()}");
        }
        /// <summary>
        /// Chase player with delay in form of coroutine in semi infinite loop
        /// </summary>
        /// <returns>
        /// Coroutines enumerator
        /// </returns>
        private IEnumerator Chase()
        {
            if (aiData.CurrentTarget == null)
            {
                PlayIdle();
                _follow = false;
                yield return null;
            }
            else
            {
                var target = contextSolver.GetDirectionToMove(steeringBehaviours, aiData);
                Move(target);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(Chase());
            }
        }
        /// <summary>
        /// Provides waiting after pause in form of coroutine
        /// </summary>
        /// <returns>
        /// Coroutines IEnumerator
        /// </returns>
        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(5f);
        }
    }
}
