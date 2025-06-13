using UnityEngine;

public class camaraFollow : MonoBehaviour
{
    public Transform target;         // Jugador a seguir
    public float fixedY = 1.5f;      // Altura fija de la cámara
    public float fixedZ = -10f;      // Profundidad fija de la cámara
    public float smoothSpeed = 5f;   // Suavidad del movimiento

    void LateUpdate()
    {
        if (target == null) return;

        // Solo seguimos la posición X del jugador
        float newX = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * smoothSpeed);
        transform.position = new Vector3(newX, fixedY, fixedZ);
    }
}
