using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT }
    public enum ToolMode { SWORD, HOE }

    Vector2 movementInput;                  // valor del moviemento
    Rigidbody2D rigidBody;                  // cuerpo del personaje
    List<RaycastHit2D> castCollisions;      // lista de colisiones que detecta el raycast?
    Animator animator;                      // animador
    SpriteRenderer spriteRenderer;          // rederizador del sprite
    bool canMove = true;                    // determina si el personaje puede moverse o no
    Direction facingDirection;              // direccion de movimiento
    Vector2 lastMoveDirection;


    public float moveSpeed;                 // velocidad de movimiento
    public ContactFilter2D movementFilter;  // filtro de colisiones
    public float collisionOffset;           // offset de la colision
    public SwordAttack swordAttack;         // ataque de espada
    public Vector2 position;                // posicion del personaje
    public Tilemap tilemap;                 // Tilemap
    public ToolMode toolMode;               // Modo de herramienta del personaje

    private readonly GUIStyle debugGuiStyle = new GUIStyle();
    private string lastAttackDirection = "";

    private void OnGUI()
    {
        debugGuiStyle.fontSize = 12;
        debugGuiStyle.fontStyle = FontStyle.Bold;

        float x = 10;
        float y = 10;

        GUI.Label(new Rect(x, y, 200, 50), $"DEBUG:");
        GUI.Label(new Rect(x, y + 15, 200, 50), $"Facing Direction: {facingDirection.ToString()}");
        GUI.Label(new Rect(x, y + 30, 200, 50), $"Last Attack Direction: {lastAttackDirection}");
        GUI.Label(new Rect(x, y + 45, 200, 50), $"Int Direction: {animator.GetInteger("direction")}");
        GUI.Label(new Rect(x, y + 60, 200, 50), $"Player Location: X: {transform.position.x * 100:F0} Y: {transform.position.y * 100:F0}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        castCollisions = new List<RaycastHit2D>();
        collisionOffset = 0.05f;
        moveSpeed = 1f;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // se llama cuando el personaje se mueve (funcion de PlayerInput)
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    // este metodo se llama con un intervalo consistente independientedel framerate (ex. f�sicas)
    private void FixedUpdate()
    {
        position = rigidBody.position;
        Movement();
        Animate();
    }

    private void Movement()
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

                // Determina la direccion en la que est� mirando el personje
                if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
                {
                    if (movementInput.x > 0)
                    {
                        facingDirection = Direction.RIGHT;
                    }
                    else
                    {
                        facingDirection = Direction.LEFT;
                    }
                }
                else
                {
                    if (movementInput.y > 0)
                    {
                        facingDirection = Direction.UP;
                    }
                    else
                    {
                        facingDirection = Direction.DOWN;
                    }
                }

                animator.SetInteger("direction", ((int)facingDirection));

                // almacena el �ltimo movimiento del personaje antes de detenerse
                lastMoveDirection = movementInput;
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // voltea el sprite en horizontal
            if (movementInput.x < 0 || lastMoveDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    // determina si el personaje se puede mover en esa direcci�n usando raycasting
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

    // se encarga de proporcionarle a los blendtrees los parametros necesarios
    void Animate()
    {
        animator.SetFloat("AnimMoveX", movementInput.x);
        animator.SetFloat("AnimMoveY", movementInput.y);
        animator.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        animator.SetFloat("AnimLastMoveY", lastMoveDirection.y);
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
        if (toolMode == ToolMode.SWORD)
        {
            switch (facingDirection)
            {
                case Direction.UP:
                    swordAttack.AttackUp();
                    lastAttackDirection = "UP";
                    break;
                case Direction.DOWN:
                    swordAttack.AttackDown();
                    lastAttackDirection = "DOWN";
                    break;
                case Direction.LEFT:
                    swordAttack.AttackLeft();
                    lastAttackDirection = "LEFT";
                    break;
                case Direction.RIGHT:
                    swordAttack.AttackRight();
                    lastAttackDirection = "RIGHT";
                    break;
            }
        }
        else
        {
            LocateCurrentFacingTile();
        }
    }

    public void StopSwordAttack()
    {
        //Debug.Log("Stopped");
        UnlockMovement();
        swordAttack.StopAttack();
    }

    // bloquea el movimiento
    public void LockMovement()
    {
        canMove = false;
    }

    // desbloquea el movimiento
    public void UnlockMovement()
    {
        canMove = true;
    }

    // Localiza la tile m�s cercana el la direcci�n del personaje y la elimina
    void LocateCurrentFacingTile()
    {
        // por la escala del tilemap y los tiles, hay transformar las coordenadas del personaje en coordenadas mas utiles
        Vector3Int currentFacingTileLocation = new Vector3Int(Mathf.FloorToInt((transform.position.x*100)/16), Mathf.FloorToInt((transform.position.y * 100) / 16), 0);
        switch (facingDirection)
        {
            // las medidas est�n hechas un poco a ojo y necesitan refinamiento
            case Direction.UP:
                //currentFacingTileLocation.y += 1;
                break;
            case Direction.DOWN:
                currentFacingTileLocation.y -= 2;
                break;
            case Direction.LEFT:
                currentFacingTileLocation.x -= 1;
                currentFacingTileLocation.y -= 1;
                break;
            case Direction.RIGHT:
                currentFacingTileLocation.x += 1;
                currentFacingTileLocation.y -= 1;
                break;
        }
        if (tilemap.GetTile(currentFacingTileLocation) != null)
        {
            tilemap.SetTile(currentFacingTileLocation, null);
        }
    }
}
