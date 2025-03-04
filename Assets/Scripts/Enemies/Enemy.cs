using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivesSystem), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float repulseForce;

    [Header("Audio")]
    [SerializeField] protected AudioClip hitSound;
    [SerializeField] protected AudioClip deadSound;

    protected Vector3 actualDestination;
    protected int actualIndex = 0;
    protected float actualPatrolSpeed;

    protected LivesSystem livesSystem;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected AudioSource audioSource;

    public Transform[] WayPoints { get => wayPoints; set => wayPoints = value; }

    private void Awake()
    {
        livesSystem = GetComponent<LivesSystem>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        actualDestination = wayPoints[actualIndex].position;
        actualPatrolSpeed = patrolSpeed;

        LookAtDestination();
        StartCoroutine(Patrol());
    }

    protected abstract IEnumerator Patrol();

    protected void LookAtDestination()
    {
        if (actualDestination.x >= transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected void TakeDamage(float damageAmount)
        => livesSystem.TakeDamage(damageAmount);


    protected void SetNewDestination()
    {
        actualIndex = ++actualIndex % wayPoints.Length;
        actualDestination = wayPoints[actualIndex].position;
        LookAtDestination();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitBox"))
        {
            if (livesSystem.Lives > 0)
                Attack(collision.GetComponent<Player>());
        }
    }

    protected abstract void Attack(Player player);
    protected abstract void Die();
    public abstract void Hit(float amountDammage, Vector2 direction);

    // Call by animation event
    protected void EndDeadAnimation()
        => Destroy(gameObject);
}
