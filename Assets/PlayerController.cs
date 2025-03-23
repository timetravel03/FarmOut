using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rigidBody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;

    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    // este metodo se llama con un intervalo consistente independientemente del framerate (normalmente se usa para físicas)
    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            // comprobamos si se puede mover en la direccion que indica el movimiento
            bool success = TryMove(movementInput);
            if (!success)
            {
                // comprobamos si se puede mover en el componente x de ese movimiento
                success = TryMove(new Vector2(movementInput.x, 0));
                if (!success)
                {
                    //comprobamos si se puede mover en el componente y de ese movimiento
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
            animator.SetBool("isMoving", success);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        // direccion del sprite
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        } else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        //comprueba si hay colisiones haciendo raycasting
        int count = rigidBody.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            //mueve el personaje
            rigidBody.MovePosition(rigidBody.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            // si hay colisiones no se mueve
            return false;
        }
    }
}
