using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // valor del moviemento
    Vector2 movementInput;
    // cuerpo del personaje
    Rigidbody2D rigidBody;
    // lista de colisiones que detecta el raycast?
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // animador
    Animator animator;
    // rederizador del sprite
    SpriteRenderer spriteRenderer;

    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    bool canMove = true;
    public SwordAttack swordAttack;
    public Vector2 position;

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
        position = rigidBody.position;
    }

    // se llama cuando el personaje se mueve (funcion de PlayerInput)
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    // este metodo se llama con un intervalo consistente independientedel framerate (ex. físicas)
    private void FixedUpdate()
    {
        if (canMove)
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
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // direccion del sprite
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    // determina si el personaje se puede mover en esa dirección usando raycasting
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

    // activa el trigger de la anim
    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    // ataque de espada
    public void SwordAttack()
    {
        LockMovement();
        print(spriteRenderer.flipX);
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void StopSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    // bloquea el movimiento
    public void LockMovement() { canMove = false; }

    // desbloquea el movimiento
    public void UnlockMovement()
    {
        canMove = true;
    }
}
