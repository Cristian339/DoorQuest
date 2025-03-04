using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Enemy
{
    public override void Hit(float damageAmount, Vector2 direction)
    {
        rb.isKinematic = false;
        rb.AddForce(direction * repulseForce, ForceMode2D.Impulse);
        TakeDamage(damageAmount);
        rb.isKinematic = true;

        if (livesSystem.Lives > 0)
        {
            animator.SetTrigger("hit");
        }
        else
        {
            Die();
        }
    }

    protected override void Attack(Player player)
    {
        player.TakeDamage(attackDamage);
        audioSource.PlayOneShot(hitSound);
        Destroy(gameObject);
    }

    protected override void Die()
    {
        audioSource.PlayOneShot(hitSound);
        animator.SetTrigger("die");
    }

    protected override IEnumerator Patrol()
    {
        while (true)
        {
            while (transform.position != actualDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, actualDestination, actualPatrolSpeed * Time.deltaTime);
                yield return null;
            }

            SetNewDestination();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectionPlayer"))
        {
            actualDestination = collision.transform.position;
        }
        else if (collision.gameObject.CompareTag("PlayerHitBox"))
        {
            Attack(collision.GetComponent<Player>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectionPlayer"))
        {
            actualDestination = collision.transform.position;
            LookAtDestination();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectionPlayer"))
        {
            SetNewDestination();
        }
    }
}
