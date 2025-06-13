using UnityEngine;

public class ActivarSonidoAlEntrar : MonoBehaviour
{
    public GameObject contenedorDeSonidos; // Asigna un GameObject que tenga todos los objetos con AudioSource
    public string tagJugador = "Player";   // Tag que debe tener el jugador

    private AudioSource[] sonidosEscenario;

    private void Start()
    {
        if (contenedorDeSonidos != null)
        {
            // Obtiene todos los AudioSource en el contenedor y sus hijos
            sonidosEscenario = contenedorDeSonidos.GetComponentsInChildren<AudioSource>();

            // Asegúrate de que no estén sonando al inicio
            foreach (AudioSource sonido in sonidosEscenario)
            {
                sonido.Stop();
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado el contenedor de sonidos.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador) && sonidosEscenario != null)
        {
            foreach (AudioSource sonido in sonidosEscenario)
            {
                if (!sonido.isPlaying)
                    sonido.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador) && sonidosEscenario != null)
        {
            foreach (AudioSource sonido in sonidosEscenario)
            {
                if (sonido.isPlaying)
                    sonido.Stop(); // O usa .Pause() si luego quieres retomarlos
            }
        }
    }
}
