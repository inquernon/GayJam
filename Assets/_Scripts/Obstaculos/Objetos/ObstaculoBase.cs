// Clase abstracta. Todo obstáculo hereda de aquí.
// Maneja la suscripción a TimeManager y la colisión con el jugador.
// Cada hijo implementa estados(EstadoPresente() y EstadoPasado()).
// REQUIERE!!: Collider con IsTrigger = true en el GameObject.

using UnityEngine;

public abstract class ObstaculoBase : MonoBehaviour
{
    protected bool enEstadoPasado = false;

    // Cuántos carriles ocupa en estado presente y pasado
    // Se usa en ObstacleSpawner para saber dónde puede spawnear
    public int carrilesPresente = 1;
    public int carrilesPasado = 1;

    protected virtual void Update()
    {
        if (GameManager.Instance.EstadoActual != GameManager.Estado.Jugando) return;
        transform.position += ScrollManager.Instance.GetDesplazamiento();
        if (transform.position.z < -10f) Destroy(gameObject);
    }

    protected virtual void Start()
    {
        TimeManager.Instance.OnPoderActivado += EstadoPasado;
        TimeManager.Instance.OnPoderDesactivado += EstadoPresente;

        if (TimeManager.Instance.PoderActivo) EstadoPasado();
        else EstadoPresente();
    }

    protected virtual void OnDestroy()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnPoderActivado -= EstadoPasado;
        TimeManager.Instance.OnPoderDesactivado -= EstadoPresente;
    }

    // Bloquea el paso — implementar en cada hijo
    protected abstract void EstadoPresente();

    // Estado alterado por el poder — implementar en cada hijo
    protected abstract void EstadoPasado();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (enEstadoPasado) return;
        if (other.CompareTag("Player"))
            GameManager.Instance.PlayerMurio();
    }
}