using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Collider2D doorCoolidder;

    public Direction dir;
    private bool isOpen = false;
    void FixedUpdate()
    {
        CheckFoeOpen();
    }

    private void CheckFoeOpen()
    {
        if (isOpen) return;
        if(GameObject.FindGameObjectsWithTag("Turret").Length == 0 && GameObject.FindGameObjectsWithTag("MeleeEnemy").Length == 0)
        {
            isOpen = true;
            renderer.sprite = openSprite;
            doorCoolidder.isTrigger = true;
            GameStateController.instance.isSwitchingRoom = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameStateController.instance.SwitchRooms(dir);

    }
}
