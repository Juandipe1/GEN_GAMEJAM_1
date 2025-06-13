using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Fuerza inicial")]
    public Vector3 spawnForce = new Vector3(1f, 2f, 1f);

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (animator != null)
            animator.SetTrigger("spawn");  // O el trigger que necesites

        if (rb != null)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDir * spawnForce.magnitude, ForceMode.Impulse);
        }
    }
}
