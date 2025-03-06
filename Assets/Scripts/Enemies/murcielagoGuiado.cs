using UnityEngine;
using System.Collections; // Añade esto para usar IEnumerator

public class murcielagoGuiado : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveDistance = 5.0f; // Distancia que se moverá el enemigo
    public float speed = 2.0f; // Velocidad de movimiento

    [Header("Referencias")]
    private Rigidbody2D rb; // Componente Rigidbody2D
    private Animator animator; // Componente Animator

    private Vector2 startPosition; // Posición inicial del enemigo
    private Vector2 targetPosition; // Posición objetivo del enemigo
    private bool movingRight = true; // Controla la dirección del movimiento

    void Start()
    {
        // Obtén las referencias a los componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Guarda la posición inicial del enemigo
        startPosition = transform.position;

        // Calcula la posición objetivo inicial (derecha)
        targetPosition = startPosition + Vector2.right * moveDistance;
    }

    void Update()
    {
        // Mueve al enemigo hacia la posición objetivo
        Move();

        // Actualiza la animación de movimiento
        animator.SetBool("enMovimiento", true);
    }

    void Move()
    {
        // Mueve al enemigo hacia la posición objetivo
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Si el enemigo alcanza la posición objetivo, cambia de dirección
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChangeDirection();
        }

        // Gira al enemigo según la dirección del movimiento
        if (movingRight)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mira a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mira a la izquierda
        }
    }

    void ChangeDirection()
    {
        // Cambia la dirección del movimiento
        movingRight = !movingRight;

        // Calcula la nueva posición objetivo
        if (movingRight)
        {
            targetPosition = startPosition + Vector2.right * moveDistance;
        }
        else
        {
            targetPosition = startPosition + Vector2.left * moveDistance;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja la distancia de movimiento en el editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * moveDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * moveDistance);
    }
}



