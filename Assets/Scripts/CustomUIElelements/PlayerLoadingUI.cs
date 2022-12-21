using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadingUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("player_go_right");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
