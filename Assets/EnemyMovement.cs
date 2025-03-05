using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float detectiunRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (detectiunRadius > distanceToPlayer)
        {
            Vector2 direction = (player.position - transform.position).normalized;


            if(direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if(direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

                movement = new Vector2(direction.x, 0);

            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        animator.SetBool("enMovimiento", enMovimiento);
    }



    void OnDragGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectiunRadius);
    }
}
