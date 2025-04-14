using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D slimeBody;
    Transform player;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    float deltaCounter = 0;
    Vector2 randomDirection;
    bool tracking;

    public ContactFilter2D movementFilter;
    public float health = 1;
    public float moveSpeed = 0.5f;
    // usado para que el slime Origin no se mueva
    public bool stationary = false;
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Vector2 directionToPlayer = (Vector2)player.position - slimeBody.position;
        //slimeBody.MovePosition(slimeBody.position - directionToPlayer * 0.5f);
        //print(Health.ToString());
    }

    public void Defeated()
    {
        animator.SetBool("defeated", true);
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        slimeBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        randomDirection = new Vector2((float)Random.Range(-1, 2), (float)Random.Range(-1, 2));
    }

    // Update is called once per frame
    //void Update()
    //{
    //    deltaCounter += Time.deltaTime;
    //    //print(deltaCounter.ToString());

    //    FollowFunction();
    //    WanderFunction();
    //}
    void FixedUpdate()
    {
        deltaCounter += Time.fixedDeltaTime; // Usar fixedDeltaTime en lugar de deltaTime
        FollowFunction();
        WanderFunction();
    }

    void FollowFunction()
    {
        // Calcular la dirección hacia el jugador
        Vector2 directionToPlayer = (Vector2)player.position - slimeBody.position;

        if (directionToPlayer.magnitude < 1f && !animator.GetBool("defeated"))
        {
            tracking = true;

            // Normalizar la dirección para mantener una velocidad constanteddsds
            directionToPlayer.Normalize();

            //if (TryMove(directionToPlayer))
            {
                // Mover el enemigo en dirección al jugador con una velocidad constante
                slimeBody.MovePosition(slimeBody.position + directionToPlayer * (moveSpeed * 0.1f) * Time.fixedDeltaTime);
                animator.SetBool("moving", true);
            }
        }
        else
        {
            tracking = false;
        }
    }

    void WanderFunction()
    {
        // cada dos segundos cambia de dirección
        if (deltaCounter >= 2f)
        {
            randomDirection = new Vector2(Random.Range(-1f, 2f), Random.Range(-1f, 2f));
            deltaCounter = 0;
        }
        else if (!tracking) // si no esta persiguiendo al jugador, diambula
        {
            if (randomDirection.x < 0.5f && randomDirection.x > -0.5f && randomDirection.y < 0.5f && randomDirection.y > -0.5f)
            {
                animator.SetBool("moving", false);
            }
            else
            {
                slimeBody.MovePosition(slimeBody.position + randomDirection * (moveSpeed * 0.1f) * Time.fixedDeltaTime);
                animator.SetBool("moving", true);
            }
        }

    }

    // TODO
    private bool TryMove(Vector2 direction)
    {

        //comprueba si hay colisiones haciendo raycasting
        int count = slimeBody.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + 0.05f);
        if (count == 0)
        {
            //mueve el personaje
            return true;
        }
        else
        {
            // si hay colisiones no se mueve
            return false;
        }
    }

}
