using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D slimeBody;
    Transform player;
    float health = 5;
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
        slimeBody.MovePosition(slimeBody.position - directionToPlayer * 0.5f);
        print(Health.ToString());
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
    }

    // Update is called once per frame
    void Update()
    {
        FollowFunction();
    }

    void FollowFunction()
    {
        // Calcular la dirección hacia el jugador
        Vector2 directionToPlayer = (Vector2)player.position - slimeBody.position;

        if (directionToPlayer.magnitude < 1f && !animator.GetBool("defeated"))
        {
            animator.SetBool("moving", true);
            // Normalizar la dirección para mantener una velocidad constante
            directionToPlayer.Normalize();

            // Mover el enemigo en dirección al jugador con una velocidad constante
            float moveSpeed = 2f; // Velocidad de movimiento
            slimeBody.MovePosition(slimeBody.position + directionToPlayer * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

}
