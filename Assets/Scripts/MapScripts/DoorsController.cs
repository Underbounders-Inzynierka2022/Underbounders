using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Collider2D doorCoolidder;

    public Direction dir;

    void FixedUpdate()
    {
        CheckFoeOpen();
    }

    private void CheckFoeOpen()
    {
        if(GameObject.FindGameObjectsWithTag("Turret").Length == 0 && GameObject.FindGameObjectsWithTag("MeleeEnemy").Length == 0)
        {
            renderer.sprite = openSprite;
            doorCoolidder.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameStateController.instance.SwitchRooms(dir);

    }
}
