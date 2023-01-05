using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public bool _isChestOpen = false;
    public (int x, int y) chestPos;
    [SerializeField] private Sprite _chestClosed;
    [SerializeField] private Sprite _chestOpen;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private string _swordHitboxTag;

    [SerializeField] private List<ItemSpawnRate> items;
  


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(_swordHitboxTag))
            OpenChest();
    }

    public void OpenChest()
    {
        ChangeSprite();
        SpawnItem();
        GameStateController.instance.currentRoom.ChestOpened.Add(chestPos);
    }

    public void SpawnItem()
    {
        List<int> spawned = new List<int>();
        foreach (var item in items)
        {
            if (HelperFunctions.RandomWeighted(new List<float>() { item.chance, 1 - item.chance }) == 0)
            {
                Instantiate(item.item,transform.position,transform.rotation);
            }
        }

        
    }

    public void ChangeSprite() {

        _renderer.sprite = _chestOpen;
        _collider.enabled = false;
        _isChestOpen = true;
    }
        
    
}
