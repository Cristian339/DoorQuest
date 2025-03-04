using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesSystem : MonoBehaviour
{
    public Action OnDie;

    [SerializeField] private float lives;

    private float maxLives;

    public float Lives { get => lives; set => lives = value; }

    private void Awake()
    {
        maxLives = lives;
    }

    public void TakeDamage(float damageAmount)
    {
        lives -= damageAmount;

        if (lives <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void AddHealth(float healthAmount)
        => lives = Mathf.Clamp(lives + healthAmount, 0, maxLives);
}
