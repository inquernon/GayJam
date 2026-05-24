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
    }

    public void IniciarJuego()
    {
        Time.timeScale = 1f;
        SetEstado(Estado.Jugando);
        ScrollManager.Instance.Iniciar();
    }

    public void PlayerMurio()
    {
        if (EstadoActual == Estado.Muerto) return;
        SetEstado(Estado.Muerto);
        ScrollManager.Instance.Detener();
        Time.timeScale = 0f; // pausa el juego, el usuario decide
    }

    // Llamado por botón Reiniciar en pantalla GameOver
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Llamado por botón Salir en pantalla GameOver
    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void SetEstado(Estado nuevo)
    {
        EstadoActual = nuevo;
        OnEstadoCambiado?.Invoke(nuevo);
    }
}