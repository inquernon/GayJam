// Sigue al jugador y hace pan suave izquierda/derecha cada cierta distancia.
// Simula la ilusión de curva sin spline real.
// Reemplaza o complementa el componente de cámara actual.

using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform jugador;
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float suavizado = 8f;

    [Header("Pan (ilusión de curva)")]
    public float anguloMaxPan = 15f;  // grados máximos de rotación Y
    public float distanciaPan = 80f;  // cada cuántos metros cambia dirección
    public float velocidadPan = 1.5f; // qué tan rápido rota

    private float anguloObjetivo = 0f;
    private float distanciaUltimoPan = 0f;

    void LateUpdate()
    {
        if (GameManager.Instance.EstadoActual != GameManager.Estado.Jugando) return;

        SeguirJugador();
        ActualizarPan();
    }

    void SeguirJugador()
    {
        Vector3 posObjetivo = jugador.position + offset;
        transform.position = Vector3.Lerp(transform.position, posObjetivo,
                                           Time.deltaTime * suavizado);
    }

    void ActualizarPan()
    {
        float distancia = ScrollManager.Instance.GetDistancia();

        // Cada distanciaPan metros elige un nuevo ángulo objetivo
        if (distancia - distanciaUltimoPan >= distanciaPan)
        {
            distanciaUltimoPan = distancia;
            anguloObjetivo = Random.Range(-anguloMaxPan, anguloMaxPan);
        }

        // Rota suavemente hacia el ángulo objetivo
        Quaternion rotObjetivo = Quaternion.Euler(0f, anguloObjetivo, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotObjetivo,
                                                   Time.deltaTime * velocidadPan);
    }
}