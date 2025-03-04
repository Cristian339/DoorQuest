using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private AudioClip attackSound;

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
            actualPatrolSpeed = 0;
            Die();
        }
    }

    protected override void Attack(Player player)
    {
        audioSource.PlayOneShot(attackSound);
        animator.SetTrigger("attack");
        player.TakeDamage(attackDamage);
    }

    protected override void Die()
    {
        audioSource.PlayOneShot(deadSound);
        animator.SetTrigger("die");
        actualPatrolSpeed = 0;
    }

    protected override IEnumerator Patrol()
    {
        while (true)
        {
            while (transform.position != actualDestination)
            {
                Vector2 newPosition = Vector3.MoveTowards(transform.position, actualDestination, actualPatrolSpeed * Time.deltaTime);
                transform.position = new Vector3(newPosition.x, transform.position.y);
                yield return null;
            }

            SetNewDestination();
        }
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }
}
