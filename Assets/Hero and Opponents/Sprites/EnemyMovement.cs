using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float detectionRadius = 5f; // Radio de detección del jugador
    public float movementRadius = 10f; // Radio máximo donde el enemigo puede moverse
    public float moveSpeed = 2f; // Velocidad del enemigo
    public Transform player; // Referencia al jugador

    private bool isPlayerInRange = false; // Para saber si el jugador está en el radio de detección

    private void Update()
    {
        // Comprobar si el jugador está dentro del radio de detección
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= detectionRadius;

        if (isPlayerInRange)
        {
            // Persigue al jugador si está dentro del radio de detección
            PursuePlayer(distanceToPlayer);
        }
    }

    private void PursuePlayer(float distanceToPlayer)
    {
        // Limitar el movimiento del enemigo dentro del radio de movimiento
        if (distanceToPlayer <= movementRadius)
        {
            // Movimiento hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja los radios de detección y movimiento en el editor para depuración
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Radio de detección

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, movementRadius); // Radio de movimiento
    }
}
