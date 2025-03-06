using UnityEngine;
using System.Collections; // Añade esto para usar IEnumerator

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
    private bool enMovimiento = true;
    private bool stop = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (stop)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Si el enemigo está demasiado lejos, no se mueve ni ataca
            if (distanceToPlayer > detectionRadius)
            {
                enMovimiento = false;
            }
            else if (distanceToPlayer < detectionRadius)
            {
                enMovimiento = true;
                MoveTowardsPlayer();
                // Si está en el rango de ataque, ataca
                if (distanceToPlayer <= attackRadius)
                {
                    enMovimiento = false;
                    AttackPlayer();
                }
            }

            // Actualiza la animación de movimiento
            animator.SetBool("enMovimiento", enMovimiento);
        }
        else
        {
            animator.SetBool("enMovimiento", false);
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
            // Activa la animación de ataque por un instante
            animator.SetBool("atacar", true);

            // Aplica el daño al jugador
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }

            // Desactiva el ataque y comienza el cooldown
            canAttack = false;
            stop = false;

            // Desactiva la animación de ataque en el siguiente frame
            StartCoroutine(ResetAttackAnimationAfterDelay(0.7f)); // Ajusta el tiempo si es necesario

            // Reinicia el ataque después del cooldown
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    IEnumerator ResetAttackAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        animator.SetBool("atacar", false); // Desactiva la animación de ataque
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