using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    private enum State { Idle, Rotating, Moving, Chasing }
    private State state;

    [Header("Audio")]
    public AudioClip enemyWalking;
    public AudioClip enemyAttack;
    private AudioSource audioSource;

    [Header("Target")]
    public Transform target;

    [Header("Roaming Settings")]
    public float idleMinTime = 2f;
    public float idleMaxTime = 4f;
    public float moveMinTime = 2f;
    public float moveMaxTime = 4f;

    [Header("Detection")]
    public float detectionRadius = 5f;

    [Header("Speeds")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float rotationSpeed = 120f;

    [Header("Health UI")]
    public Slider healthBarSlider;
    public int maxHealth = 10;
    private int currentHealth;

    private float stateTimer, stateDuration;
    private Quaternion roamTargetRotation;
    //private Animator animator;

    [Header("Attack Settings")]
    public float Range = 1.5f; // Rango corto para ataque cuerpo a cuerpo
    public float attackCooldown = 1f; // Tiempo entre ataques
    public int damageAmount = 1; // Da�o que hace al jugador
    public string attackAnimationTrigger = "attack"; // Trigger que debe tener la animaci�n de ataque
    private float lastAttackTime;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        //animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Inicializo el audio
        if (target == null)
        {
            var go = GameObject.Find("Player");
            if (go != null) target = go.transform;
        }

        currentHealth = maxHealth;
        if (healthBarSlider != null) healthBarSlider.value = 1f;
        SetStateIdle();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, target.position);

        // Si est� dentro del rango de detecci�n, lo persigue
        if (dist <= detectionRadius)
        {
            if (state != State.Chasing)
            {
                state = State.Chasing;
                //animator.SetBool("walk", false);
                //animator.SetBool("run", true);
            }
        }
        else if (state == State.Chasing)
        {
            SetStateIdle();
        }

        // Estados del enemigo
        switch (state)
        {
            case State.Idle:
                //animator.SetBool("walk", false);
                //animator.SetBool("run", false);
                stateTimer += Time.deltaTime;

                if (audioSource.isPlaying) audioSource.Stop(); // No suena nada en Idle

                if (stateTimer >= stateDuration)
                {
                    float angle = Random.Range(0f, 360f);
                    roamTargetRotation = Quaternion.Euler(0, angle, 0);
                    state = State.Rotating;
                    stateTimer = 0f;
                }
                break;

            case State.Rotating:
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    roamTargetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (audioSource.isPlaying) audioSource.Stop(); // Silencio al rotar

                if (Quaternion.Angle(transform.rotation, roamTargetRotation) < 1f)
                    SetStateMoving();
                break;

            case State.Moving:
                //animator.SetBool("walk", true);
                //animator.SetBool("run", false);
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);

                // Sonido de caminar mientras patrulla
                if (!audioSource.isPlaying && enemyWalking != null)
                    audioSource.PlayOneShot(enemyWalking);

                stateTimer += Time.deltaTime;
                if (stateTimer >= stateDuration) SetStateIdle();
                break;

            case State.Chasing:
                Vector3 dir = target.position - transform.position;
                dir.y = 0;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    lookRot,
                    rotationSpeed * Time.deltaTime
                );

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget <= Range)
                {
                    TryMeleeAttack(); // Ataca si est� muy cerca
                }
                else
                {
                    transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);

                    // Sonido de caminar mientras persigue
                    if (!audioSource.isPlaying && enemyWalking != null)
                        audioSource.PlayOneShot(enemyWalking);
                }
                break;
        }
    }

    private void SetStateIdle()
    {
        state = State.Idle;
        stateTimer = 0f;
        stateDuration = Random.Range(idleMinTime, idleMaxTime);
    }

    private void SetStateMoving()
    {
        state = State.Moving;
        stateTimer = 0f;
        stateDuration = Random.Range(moveMinTime, moveMaxTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword")) TakeDamage(2);
        else if (other.CompareTag("Scream")) TakeDamage(3);
    }

    void TakeDamage(int dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        if (healthBarSlider != null)
            healthBarSlider.value = (float)currentHealth / maxHealth;

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        TakeDamage(2);
    }

    // Este m�todo se llama cuando el enemigo est� dentro del rango de ataque
    void TryMeleeAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            //animator.SetTrigger(attackAnimationTrigger); // Reproduzco la animaci�n de ataque

            if (enemyAttack != null)
                audioSource.PlayOneShot(enemyAttack); // Sonido de ataque

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // Llama al m�todo TakeDamage en el jugador si lo tiene
                playerHealth.TakeDamage(1);
            }
        }
    }

    // Gizmos para ver los rangos en el editor
    void OnDrawGizmosSelected()
    {
        // Rango de detecci�n (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Rango de ataque (morado)
        Gizmos.color = new Color(0.6f, 0f, 0.6f, 1f);
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
