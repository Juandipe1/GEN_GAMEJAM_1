using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] CinemachineCamera activeCamera;
    [SerializeField] Collider coll;

    void Update()
    {
        coll.enabled = false;
        coll.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCamera.Priority = 1;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCamera.Priority = 0;
        }
    }
}
