using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementInput;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private ContactFilter2D moveFilter;
    [SerializeField] private float collisionOffSet = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(movementInput != Vector2.zero)
        {
            var isMoving = TryMove(movementInput);
            animator.SetBool("IsMoving", isMoving);
            if (movementInput.x < 0)
            {
                animator.SetInteger("Direction", 2);
            }
            else if(movementInput.x > 0)
            {
                animator.SetInteger("Direction", 0);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

    }

    private bool TryMove(Vector2 Direction)
    {
        int countOfColisions = rigidbody.Cast(movementInput, moveFilter, collisions, moveSpeed * Time.fixedDeltaTime + collisionOffSet);
        if (countOfColisions == 0)
        {
            rigidbody.MovePosition(rigidbody.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }


}
