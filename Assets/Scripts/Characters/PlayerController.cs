using Assets;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT }
    public enum ToolMode { SWORD, HOE, WATERING, PICKAXE }

    Vector2 movementInput;                  // valor del moviemento
    Rigidbody2D rigidBody;                  // cuerpo del personaje
    List<RaycastHit2D> castCollisions;      // lista de colisiones que detecta el raycast?
    Animator animator;                      // animador
    SpriteRenderer spriteRenderer;          // rederizador del sprite
    bool canMove = true;                    // determina si el personaje puede moverse o no
    Direction facingDirection;              // direccion de movimiento
    Vector2 lastMoveDirection;


    public CropManager cropManager;
    public float moveSpeed;                 // velocidad de movimiento
    public ContactFilter2D movementFilter;  // filtro de colisiones
    public float collisionOffset;           // offset de la colision
    public SwordAttack swordAttack;         // ataque de espada
    public HoeTool hoeTool;                 // script de funcionamiento de la azada
    public GameObject cropHelper;           // un cuadrado que muestra el tile que se esta seleccionando
    public Vector2 position;                // posicion del personaje
    public Tilemap farmlandTilemap;         // Tilemap
    public ToolMode toolMode;               // Modo de herramienta del personaje

    private readonly GUIStyle debugGuiStyle = new GUIStyle();
    private string lastAttackDirection = "";
    private int toolIndex;
    private ToolMode[] tools = new ToolMode[] { ToolMode.SWORD, ToolMode.HOE, ToolMode.WATERING };

    public Item addItemTest;

    public static bool LockFireEvent = false;
    public static PlayerController instance;

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
        GUI.Label(new Rect(x, y + 75, 200, 50), $"Tool Mode: {toolMode.ToString()}");
        GUI.Label(new Rect(x, y + 120, 200, 50), $"Clickable: {GlobalVariables.CursorOverClickableObject}");

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
        toolIndex = 0;

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (toolIndex < tools.Length - 1)
            {
                toolIndex++;
            }
            else
            {
                toolIndex = 0;
            }

            toolMode = tools[toolIndex];
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            InventoryManager.instance.AddItem(addItemTest);
        }
    }

    // se llama cuando el personaje se mueve (funcion de PlayerInput)
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    // este metodo se llama con un intervalo consistente independiente del framerate (ex. físicas)
    private void FixedUpdate()
    {
        position = rigidBody.position;
        if (!GlobalVariables.LockPlayerMovement)
        {
            Movement();
        }
        Animate();

        // TODO revisar comprobación
        if (InventoryManager.instance.GetSelectedItem(false)?.actionType == Item.ActionType.Till 
            || InventoryManager.instance.GetSelectedItem(false)?.actionType == Item.ActionType.Plant)
        {
            cropHelper.GetComponent<SpriteRenderer>().enabled = true;
            cropHelper.transform.position = farmlandTilemap.CellToWorld(LocateCurrentFacingTile()) + new Vector3(0.08f, 0.08f);
        }
        else
        {
            cropHelper.GetComponent<SpriteRenderer>().enabled = false;
        }
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

                // Determina la direccion en la que está mirando el personje
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

                // almacena el último movimiento del personaje antes de detenerse
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
        if (!GlobalVariables.CursorOverClickableObject)
        {
            switch (InventoryManager.instance.GetSelectedItem(false)?.actionType)
            {
                case Item.ActionType.Attack:
                    animator.SetTrigger("swordAttack");
                    break;
                case Item.ActionType.Harvest:

                    break;
                case Item.ActionType.Break:
                    animator.SetTrigger("pickAction");
                    break;
                case Item.ActionType.Plant:
                    animator.SetTrigger("hoeAction");
                    break;
                case Item.ActionType.Till:
                    if (InventoryManager.instance.GetSelectedItem(false)?.type == Item.ItemType.Hoe)
                    {
                        animator.SetTrigger("hoeAction");
                    }
                    else if (InventoryManager.instance.GetSelectedItem(false)?.type == Item.ItemType.WaterCan)
                    {
                        animator.SetTrigger("waterAction");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    // ataque de espada
    public void ToolBasedInteraction()
    {
        LockMovement();
        Vector3Int tempPosition = LocateCurrentFacingTile();
        Item selectedItem = InventoryManager.instance.GetSelectedItem(false);

        switch (selectedItem?.actionType)
        {
            case Item.ActionType.Attack:
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
                break;
            case Item.ActionType.Plant:
                if (cropManager.PlantCrop(tempPosition, CropManager.TypeToCrop(selectedItem.type)))
                {
                    // reduce el numero de objetos
                    InventoryManager.instance.GetSelectedItem(true);
                }
                break;
            case Item.ActionType.Till:
                if (selectedItem.type == Item.ItemType.Hoe)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (!cropManager.RemoveCrop(tempPosition))
                        {
                            cropManager.RemoveFarmland(tempPosition);
                        }
                    }
                    else
                    {
                        cropManager.HarvestCrop(tempPosition);
                        cropManager.CreateFarmland(tempPosition);
                    }
                }
                else if (selectedItem.type == Item.ItemType.WaterCan)
                {
                    cropManager.WaterTile(tempPosition);
                }
                break;
            default:
                break;
        }
    }

    public void StopSwordAttack()
    {
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

    // Localiza la tile más cercana el la dirección del personaje
    Vector3Int LocateCurrentFacingTile()
    {
        Vector3Int currentFacingTileLocation = farmlandTilemap.WorldToCell(transform.position);
        switch (facingDirection)
        {
            // hecho a ojo
            case Direction.UP:
                //currentFacingTileLocation.y += 1;
                break;
            case Direction.DOWN:
                currentFacingTileLocation.y -= 1;
                break;
            case Direction.LEFT:
                currentFacingTileLocation.x -= 1;
                //currentFacingTileLocation.y -= 1;
                break;
            case Direction.RIGHT:
                currentFacingTileLocation.x += 1;
                //currentFacingTileLocation.y -= 1;
                break;
        }

        return currentFacingTileLocation;
    }
}
