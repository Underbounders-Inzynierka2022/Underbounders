using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private Sprite _chestClosed;
    [SerializeField] private Sprite _chestOpen;
    [SerializeField] private bool _isChestOpen = false;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private string _swordHitboxTag;
  


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
        _isChestOpen = true;
        _collider.enabled = false;
        _renderer.sprite = _chestOpen;
    }

}
