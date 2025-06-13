using System;
using UnityEngine;

public class ZoneActivator : MonoBehaviour
{
    public int indiceSeccion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EscenarioManager manager = FindObjectOfType<EscenarioManager>();
            if (manager != null)
            {
                manager.ActivarSoloTresSecciones(indiceSeccion);
            }
        }
    }
}
