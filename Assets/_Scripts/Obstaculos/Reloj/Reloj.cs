// Objeto que recarga el TimeManager al colisionar con el jugador.
// Se mueve con el mundo usando ScrollManager.
// SETUP:
//   Reloj (GameObject)
//   ├── Reloj.cs
//   ├── Collider (IsTrigger = true)
//   └── MeshRenderer (modelo de reloj)

using UnityEngine;

public class Reloj : MonoBehaviour
{
    public float cargaQueOtorga = 30f;

    void Update()
    {
        // Se mueve con el mundo igual que los planos
        transform.position += ScrollManager.Instance.GetDesplazamiento();

        if (transform.position.z < -10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TimeManager.Instance.AgregarCarga(cargaQueOtorga);
        Destroy(gameObject);
    }
}