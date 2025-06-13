using UnityEngine;
using System.Collections;

public class PlayerTeleporter : MonoBehaviour
{
    public Transform[] spawnPoints; // Asigna 16 puntos en el Inspector
    public string portalTag = "Portal"; // Aseg�rate que el portal tenga este tag

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(portalTag))
        {
            Debug.Log("Entró al portal");
            StartCoroutine(TeleportWithDelay());
        }
    }

    IEnumerator TeleportWithDelay()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        yield return null; // esperar un frame

        TeleportToRandomSpawn();

        if (cc != null)
            cc.enabled = true;
    }

    void TeleportToRandomSpawn()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawn = spawnPoints[index];

        transform.position = selectedSpawn.position;
        Debug.Log("Teletransportado a: " + selectedSpawn.name);
    }
}
