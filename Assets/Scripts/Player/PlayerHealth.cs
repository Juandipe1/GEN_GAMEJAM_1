using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    public Slider healthBarSlider; // Asignar el slider desde el Inspector

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    // M�todo que puede ser llamado por otros scripts para hacer da�o
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log(currentHealth);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        RestartScene(); // Reiniciamos la escena
    }


    void RestartScene()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
