using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    //daño
    public float damage = 3;

    //collider de la espada (lados)
    public Collider2D swordSideCollider;

    //collider de la espada (arriba)
    public Collider2D swordTopCollider;

    //collider de la espada (abajo)
    public Collider2D swordBottomCollider;

    // posicion del collider
    Vector2 attackOffset;

    //no usado aún
    public enum AttackDirection { Left, Right, Up, Down }
    public AttackDirection attackDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackRight()
    {
        swordSideCollider.enabled = true;
        transform.localPosition = attackOffset;
    }

    public void AttackLeft()
    {
        // activa la colision de la espada
        swordSideCollider.enabled = true;
        // darle la vuelta al ataque
        transform.localPosition = new Vector3(attackOffset.x * -1, attackOffset.y);
    }

    public void AttackUp()
    {
        swordTopCollider.enabled = true;
        transform.localPosition = new Vector3(attackOffset.x, attackOffset.y);
    }

    public void AttackDown()
    {
        swordBottomCollider.enabled = true;
        transform.localPosition = new Vector3(attackOffset.x, attackOffset.y);
    }

    public void StopAttack()
    {
        swordSideCollider.enabled = false;
        swordTopCollider.enabled = false;
        swordBottomCollider.enabled = false;
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
