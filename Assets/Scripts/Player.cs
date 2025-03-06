using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LivesSystem))]
public class Player : MonoBehaviour
{
    public Action OnDie;
    public Action OnGetKey;
    public Action OnDieByFall;

    [Header("Movement System")]
    [SerializeField] private Transform feet;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDetectionDistance;
    [SerializeField] private LayerMask jumpable;
    [SerializeField] private float repulseForceWhenVertical;
    [SerializeField] private float repulseForceWhenHorizontal;
    [SerializeField] private float yLimit;

    [Header("Attack System")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackAmount;
    [SerializeField] private LayerMask damageable;

    [Header("Others")]
    [SerializeField] private float trapsDamage;
    [SerializeField] private Slider livebar;

    [Header("Audio")]
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip playerDeadSound;
    [SerializeField] private AudioClip powerUpSound;

    private AudioSource audioSource;

    private Animator anim;
    private Rigidbody2D rb;
    private LivesSystem liveSystem;
    private float inputH;
    private float currentY;

    [SerializeField] private bool hasKey = false;
    private bool nearDoor = false;
    private Door door;

    private bool hasJump = false;
    private bool hasDie = false;

    private Vector2 positionBeforeFall;

    private bool pause = false;
    public bool Pause { get => pause; set => pause = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        liveSystem = GetComponent<LivesSystem>();
        liveSystem.OnDie += Die;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause) return;
        if (hasDie) return;

        if (transform.position.y <= yLimit)
        {
            DieForFall();
        }

        ThrowAttack();

        hasJump = !InGround();

        Jump();

        if (Input.GetKeyDown(KeyCode.E) && nearDoor)
        {
            door.Open(hasKey);
        }

        Fall();

        Movement();

        if (InGround())
        {
            positionBeforeFall = transform.position;
        }

        livebar.value = liveSystem.Lives / 100f;
    }

private void DieForFall()
{
    transform.position = positionBeforeFall;
    liveSystem.TakeDamage(trapsDamage); 
    OnDieByFall?.Invoke();
}

    public void FallInTrap()
    {
        anim.SetTrigger("hit");

        if (Mathf.Abs(rb.linearVelocity.y) > 0)
        {
            rb.AddForce(Vector3.up * repulseForceWhenVertical, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.up * repulseForceWhenHorizontal, ForceMode2D.Impulse);
        }

        liveSystem.TakeDamage(trapsDamage);
    }

    private void Fall()
    {
        bool inGround = InGround();

        if (transform.position.y >= currentY && !inGround)
        {
            anim.SetBool("falling", true);
        }
        else if (inGround)
        {
            anim.SetBool("falling", false);
        }

        currentY = transform.position.y;
    }

    private void ThrowAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }

    // Se ejecuta desde evento de animaci�n
    private void Attack()
    {
        Collider2D[] touchedColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, damageable);

        foreach (Collider2D collider in touchedColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.Hit(attackAmount, transform.right);
        }
    }

    private void PlaySwordSound()
    {
        audioSource.PlayOneShot(swordSound);
    }

    private void PlayHitSound()
    {
        if (liveSystem.Lives > 0)
            audioSource.PlayOneShot(playerHitSound);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            hasJump = true;
            audioSource.PlayOneShot(playerJumpSound);
        }
    }

    private bool InGround()
    {
        return Physics2D.Raycast(feet.position, Vector3.down, groundDetectionDistance, jumpable);
    }

    private void Movement()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(inputH * movementSpeed, rb.linearVelocity.y);

        if (inputH != 0)
        {
            anim.SetBool("running", true);
            if (inputH > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    private void Die()
    {
        hasDie = true;
        rb.linearVelocity = Vector3.zero;
        anim.SetTrigger("die");
        audioSource.PlayOneShot(playerDeadSound);
        rb.simulated = false;

        foreach (var script in GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }

        OnDie?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(0.000001f);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        CameraShake.Shake(0.45f, 10f);
        rb.AddForce(Vector3.up * repulseForceWhenVertical / 2f, ForceMode2D.Impulse);
        liveSystem.TakeDamage(damageAmount);
        anim.SetTrigger("hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            nearDoor = true;
            door = collision.GetComponent<Door>();
        }
        else if (collision.CompareTag("Key"))
        {
            audioSource.PlayOneShot(powerUpSound);
            Destroy(collision.gameObject);
            hasKey = true;
            OnGetKey?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            nearDoor = false;
        }
    }

    public void GetHealthPowerUp(float health)
    {
        audioSource.PlayOneShot(powerUpSound);
        liveSystem.AddHealth(health);
    }

    public void SetLives(float lives)
    {
        liveSystem.Lives = lives;
    }

    public float GetLives()
    {
        return liveSystem.Lives;
    }
}
