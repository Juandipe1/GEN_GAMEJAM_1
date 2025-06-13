using UnityEngine;

public class EscenarioManager : MonoBehaviour
{
    public GameObject[] secciones; // Todas las secciones del escenario en orden
    private int indiceActual = -1;

    public void ActivarSoloTresSecciones(int nuevoIndice)
    {
        if (nuevoIndice < 0 || nuevoIndice >= secciones.Length) return;

        // Guardar el nuevo índice como actual
        indiceActual = nuevoIndice;

        for (int i = 0; i < secciones.Length; i++)
        {
            // Solo activar la sección anterior, actual y siguiente
            if (i == indiceActual - 1 || i == indiceActual || i == indiceActual + 1)
            {
                secciones[i].SetActive(true);
            }
            else
            {
                secciones[i].SetActive(false);
            }
        }

        Debug.Log("Secciones activas: " + (indiceActual - 1) + ", " + indiceActual + ", " + (indiceActual + 1));
    }
}

