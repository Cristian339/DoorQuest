using UnityEngine;
using System.Collections; // A�ade esto para usar IEnumerator

public class murcielagoGuiado : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    public float moveDistance = 5.0f; // Distancia que se mover� el enemigo
    public float speed = 2.0f; // Velocidad de movimiento

    [Header("Referencias")]
    private Rigidbody2D rb; // Componente Rigidbody2D
    private Animator animator; // Componente Animator

    private Vector2 startPosition; // Posici�n inicial del enemigo
    private Vector2 targetPosition; // Posici�n objetivo del enemigo
    private bool movingRight = true; // Controla la direcci�n del movimiento

    void Start()
    {
        // Obt�n las referencias a los componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Guarda la posici�n inicial del enemigo
        startPosition = transform.position;

        // Calcula la posici�n objetivo inicial (derecha)
        targetPosition = startPosition + Vector2.right * moveDistance;
    }

    void Update()
    {
        // Mueve al enemigo hacia la posici�n objetivo
        Move();

        // Actualiza la animaci�n de movimiento
        animator.SetBool("enMovimiento", true);
    }

    void Move()
    {
        // Mueve al enemigo hacia la posici�n objetivo
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Si el enemigo alcanza la posici�n objetivo, cambia de direcci�n
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChangeDirection();
        }

        // Gira al enemigo seg�n la direcci�n del movimiento
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
        // Cambia la direcci�n del movimiento
        movingRight = !movingRight;

        // Calcula la nueva posici�n objetivo
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



