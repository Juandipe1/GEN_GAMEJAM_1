using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameInput gameInput;

    void Start()
    {
        Continue();
    }

    void Update()
    {
        if (gameInput.TogglePauseIfPressed()) // solo entra si se presionó el botón
        {
            if (gameInput.IsPaused())
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
