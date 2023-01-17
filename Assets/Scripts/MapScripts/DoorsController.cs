using UnderBounders;
using UnityEngine;

namespace MapScripts
{
    /// <summary>
    /// Doors behaviour controller
    /// </summary>
    public class DoorsController : MonoBehaviour
    {
        /// <summary>
        /// Door facing direction
        /// </summary>
        public Direction dir { get; set; }


        /// <summary>
        /// Doors sprite renderer
        /// </summary>
        [SerializeField] private SpriteRenderer renderer;
        /// <summary>
        /// Open doors sprtie
        /// </summary>
        [SerializeField] private Sprite openSprite;
        /// <summary>
        /// Doors collider
        /// </summary>
        [SerializeField] private Collider2D doorCoolidder;

        /// <summary>
        /// Determines if door is open
        /// </summary>
        private bool isOpen = false;

        void FixedUpdate()
        {
            CheckForOpen();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                GameStateController.Instance.SwitchRooms(dir);

        }

        /// <summary>
        /// Opens closed doors if all monsters in the room are killed
        /// </summary>
        private void CheckForOpen()
        {
            if (isOpen) return;
            if (GameObject.FindGameObjectsWithTag("Turret").Length == 0 && GameObject.FindGameObjectsWithTag("MeleeEnemy").Length == 0)
            {
                isOpen = true;
                renderer.sprite = openSprite;
                doorCoolidder.isTrigger = true;
                GameStateController.Instance.isSwitchingRoom = false;
            }
        }
    }
}
