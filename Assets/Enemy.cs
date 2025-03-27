using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

    float health = 1;
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
        print(Health);
    }

    public void Defeated()
    {
        animator.SetTrigger("defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
