using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public bool inTrap = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox") && !inTrap)
        {
            collision.GetComponent<Player>().FallInTrap();
            inTrap = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox"))
        {
            inTrap = false;
        }
    }
}
