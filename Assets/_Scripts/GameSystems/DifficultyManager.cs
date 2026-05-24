// Aumenta velocidad del scroll y frecuencia de spawn según distancia recorrida.
// No requiere configuración de arte. Solo números en el inspector.

using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Velocidad")]
    public float velocidadMax = 20f;
    public float incrementoVelocidad = 0.5f; // unidades por cada 100m

    [Header("Spawn")]
    public float intervaloMin = 0.8f;   // intervalo mínimo entre obstáculos
    public float reduccionIntervalo = 0.1f;  // se reduce por cada 100m

    private float distanciaAnterior = 0f;

    void Update()
    {
        if (GameManager.Instance.EstadoActual != GameManager.Estado.Jugando) return;

        float distancia = ScrollManager.Instance.GetDistancia();
        float tramos = Mathf.Floor(distancia / 100f); // cada 100m sube dificultad
        float tramosAnteriores = Mathf.Floor(distanciaAnterior / 100f);

        if (tramos > tramosAnteriores)
        {
            float nuevaVelocidad = Mathf.Min(
                ScrollManager.Instance.velocidadInicial + tramos * incrementoVelocidad,
                velocidadMax
            );
            ScrollManager.Instance.SetVelocidad(nuevaVelocidad);

            float nuevoIntervalo = Mathf.Max(
                ObstacleSpawner.Instance.intervaloSpawn - reduccionIntervalo,
                intervaloMin
            );
            ObstacleSpawner.Instance.SetIntervalo(nuevoIntervalo);
        }

        distanciaAnterior = distancia;
    }
}