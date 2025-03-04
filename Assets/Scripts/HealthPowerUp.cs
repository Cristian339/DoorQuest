using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    [SerializeField] private float health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox"))
        {
            collision.GetComponent<Player>().GetHealthPowerUp(health);
            Destroy(this.gameObject);
        }
    }
}
