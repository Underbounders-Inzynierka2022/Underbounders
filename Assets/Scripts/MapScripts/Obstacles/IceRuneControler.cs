using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRuneControler : MonoBehaviour
{
    [SerializeField] private PlayerSO player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.speed = player.speed / 2;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.speed = player.speed * 2;
        }
    }
}
