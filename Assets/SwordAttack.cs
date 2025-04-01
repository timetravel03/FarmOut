using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    //daño
    public float damage = 3;

    //collider de la espada
    public Collider2D swordCollider;

    // posicion del collider
    Vector2 rightAttackOffset;

    //no usado aún
    public enum AttackDirection { Left, Right }
    public AttackDirection attackDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightAttackOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // enum util para cuando haga el resto de animaciones (arriba y abajo) (aun no usado)
    public void Attack()
    {
        switch (attackDirection)
        {
            case AttackDirection.Left:
                AttackLeft();
                break;
            case AttackDirection.Right:
                AttackRight();
                break;
            default:
                break;
        }
    }
    

    public void AttackRight()
    {
        //print("attack right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        //print("attack left");

        // activa la colision de la espada
        swordCollider.enabled = true;

        // darle la vuelta al ataque
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //dañar enemigo
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
