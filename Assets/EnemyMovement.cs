using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float attackRadius = 2.0f;
    public float speed = 2.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool canAttack = true;
    private bool atacar;
    private bool enMovimiento = true;
    private bool stop = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if(stop){
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Si el enemigo está demasiado lejos, no se mueve ni ataca
            if (distanceToPlayer > detectionRadius)
            {
                enMovimiento = false;
                atacar = false;
            }else if (distanceToPlayer < detectionRadius)
            {
                enMovimiento= true;
                MoveTowardsPlayer();
                // Si está en el rango de ataque, ataca
                    if (distanceToPlayer <= attackRadius)
                    {
                        enMovimiento = false;
                        atacar = true;
                        AttackPlayer();
                    }

            }

            // Actualiza la animación de movimiento
            animator.SetBool("enMovimiento", enMovimiento);
            animator.SetBool("atacar", atacar);

        }else{

            animator.SetBool("enMovimiento", false);
            animator.SetBool("atacar", false);
        }
    }

    void MoveTowardsPlayer()
    {
        if (!canAttack) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);  // Gira al jugador
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        if (canAttack)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null) playerScript.TakeDamage(attackDamage);

            canAttack = false;
            stop = false;


            Invoke(nameof(ResetAttack), attackCooldown);  // Espera el cooldown y luego habilita el ataque
        }
    }

    void ResetAttack()
    {
        canAttack = true;
        stop = true;
        enMovimiento = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
