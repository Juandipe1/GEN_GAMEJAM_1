using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        // Ignorar colisi�n con quien lo dispar� (suponiendo que tiene tag "Enemy")
        Collider projCol = GetComponent<Collider>();
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var c in hits)
        {
            if (c.CompareTag("Enemy"))
                Physics.IgnoreCollision(projCol, c);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // S�lo nos importa el choque contra el Player
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GetComponent<Collider>().enabled = false;
            playerHealth.TakeDamage(damage);
            Debug.Log("Hit Player");
        }
        // �No m�s Destroy() aqu�! El resto de colisiones las ignoramos.
    }
}

