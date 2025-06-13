using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyMovement2 : MonoBehaviour
{
    private enum State { Idle, Moving, Shooting }

    [Header("Audio")]
    public AudioClip EnemyShoot; // sonido que se reproduce al disparar
    public AudioClip enemyWalking; // sonido de pasos mientras camina
    private AudioSource audioSource; // referencia al componente de audio

    [Header("Target & Detection")]
    public Transform target;
    public float detectionRadius = 8f;

    [Header("Movement")]
    public float patrolSpeed = 1f;
    public float rotationSpeed = 120f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public float fireRate = 1f;

    [Header("Timing Patrol")]
    public float idleMinTime = 2f;
    public float idleMaxTime = 4f;
    public float moveMinTime = 2f;
    public float moveMaxTime = 4f;

    [Header("Health UI")]
    public Slider healthBarSlider;
    public int maxHealth = 10;
    private int currentHealth;

    private State state = State.Idle;
    private float stateTimer, stateDuration, shootCooldown;
    private Vector3 patrolDirection;
    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // obtengo el AudioSource

        if (target == null)
        {
            var go = GameObject.Find("Player");
            if (go != null) target = go.transform;
        }

        currentHealth = maxHealth;
        if (healthBarSlider != null) healthBarSlider.value = 1f;
        EnterIdle();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, target.position);

        // Cambia al estado de disparo si el jugador est� cerca
        if (dist <= detectionRadius && state != State.Shooting)
            EnterShooting();
        else if (dist > detectionRadius && state == State.Shooting)
            EnterIdle();

        // Cambia de comportamiento seg�n el estado
        switch (state)
        {
            case State.Idle: PatrolIdle(); break;
            case State.Moving: PatrolMove(); break;
            case State.Shooting: AimAndShoot(); break;
        }
    }

    #region Patrol
    void EnterIdle()
    {
        state = State.Idle;
        stateTimer = 0f;
        stateDuration = Random.Range(idleMinTime, idleMaxTime);
        //animator.SetBool("walk", false);
        //animator.SetBool("run", false);

        // En idle no debe sonar el audio de caminar
        if (audioSource.isPlaying) audioSource.Stop();
    }

    void PatrolIdle()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            float angle = Random.Range(0f, 360f);
            patrolDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            state = State.Moving;
            stateTimer = 0f;
            stateDuration = Random.Range(moveMinTime, moveMaxTime);
            //animator.SetBool("walk", true);
        }
    }

    void PatrolMove()
    {
        Quaternion targetRot = Quaternion.LookRotation(patrolDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRot,
            rotationSpeed * Time.deltaTime
        );

        transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);

        // Sonido de pasos mientras patrulla
        if (!audioSource.isPlaying && enemyWalking != null)
            audioSource.PlayOneShot(enemyWalking);

        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration) EnterIdle();
    }
    #endregion

    #region Shooting
    void EnterShooting()
    {
        state = State.Shooting;
        //animator.SetBool("walk", false);
        //animator.SetBool("run", true);
        shootCooldown = 0f;

        // Detengo cualquier sonido de pasos al cambiar al estado de disparo
        if (audioSource.isPlaying) audioSource.Stop();
    }

    void AimAndShoot()
    {
        Vector3 dir = target.position - transform.position; dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, lookRot,
                rotationSpeed * Time.deltaTime
            );
        }

        shootCooldown -= Time.deltaTime;

        if (shootCooldown <= 0f)
        {
            // Crea la bala
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
            Destroy(bullet, 5f);

            // Suena el disparo si tiene audio asignado
            if (EnemyShoot != null)
                audioSource.PlayOneShot(EnemyShoot);

            shootCooldown = 1f / fireRate;
        }
    }
    #endregion

    #region Health UI
    public void UpdateHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
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
        UpdateHealth(currentHealth - 2);
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
