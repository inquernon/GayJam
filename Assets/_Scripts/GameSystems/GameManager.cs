// Controla estados del juego: Menu, Jugando, Muerto.
// La UI escucha OnEstadoCambiado para mostrar/ocultar pantallas.

using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Estado { Menu, Jugando, Muerto }
    public Estado EstadoActual { get; private set; }
    public event Action<Estado> OnEstadoCambiado;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        SetEstado(Estado.Menu);
        //ScrollManager.Instance.Iniciar();
    }

    public void IniciarJuego()
    {
        SetEstado(Estado.Jugando);
        ScrollManager.Instance.Iniciar();
    }

    public void PlayerMurio()
    {
        if (EstadoActual == Estado.Muerto) return;
        SetEstado(Estado.Muerto);
        ScrollManager.Instance.Detener();
        Invoke(nameof(Reiniciar), 3f); //se reinicia después de 3 segundos
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void SetEstado(Estado nuevo)
    {
        EstadoActual = nuevo;
        OnEstadoCambiado?.Invoke(nuevo);
    }
}