using System.Collections.Generic;
using UnderBounders;
using UnityEngine;

namespace MapScripts.Obstacles
{
    /// <summary>
    /// Controlles chest behaviours
    /// </summary>
    public class ChestController : MonoBehaviour
    {
        /// <summary>
        /// Determines if chest is open
        /// </summary>
        public bool _isChestOpen { get; set; }
        /// <summary>
        /// Position of the chest in the room
        /// </summary>
        public (int x, int y) chestPos { get; set; }
        /// <summary>
        /// Sprite for closed chest
        /// </summary>
        [SerializeField] private Sprite chestClosed;
        /// <summary>
        /// Sprite for opened chest
        /// </summary>
        [SerializeField] private Sprite chestOpen;
        /// <summary>
        /// Chest collider
        /// </summary>
        [SerializeField] private Collider2D collider;
        /// <summary>
        /// Chest sprite renderer
        /// </summary>
        [SerializeField] private SpriteRenderer renderer;
        /// <summary>
        /// Sword attack tag
        /// </summary>
        [SerializeField] private string swordHitboxTag;
        /// <summary>
        /// Possible items to be spawn
        /// </summary>
        [SerializeField] private List<ItemSpawnRate> items;

        void Awake()
        {
            _isChestOpen = false;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(swordHitboxTag))
                OpenChest();
        }
        /// <summary>
        /// Open chests with item spawn
        /// </summary>
        public void OpenChest()
        {
            ChangeSprite();
            SpawnItem();
            GameStateController.Instance.currentRoom.ChestOpened.Add(chestPos);
        }
        /// <summary>
        /// Spawns items from chests
        /// </summary>
        public void SpawnItem()
        {
            List<int> spawned = new List<int>();
            foreach (var item in items)
            {
                if (HelperFunctions.RandomWeighted(new List<float>() { item.chance, 1 - item.chance }) == 0)
                {
                    Instantiate(item.item, transform.position, transform.rotation);
                }
            }


        }
        /// <summary>
        /// Changes sprite and opens the chest
        /// </summary>
        public void ChangeSprite()
        {

            renderer.sprite = chestOpen;
            collider.enabled = false;
            _isChestOpen = true;
        }
    }
}
