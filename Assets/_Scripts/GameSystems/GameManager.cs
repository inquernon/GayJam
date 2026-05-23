// Controla los estados del juego: jugando y muerto.
// Los obstáculos llamaran PlayerMurio() al colisionar.

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        ScrollManager.Instance.Iniciar();
    }

    public void PlayerMurio()
    {
        ScrollManager.Instance.Detener();
        // Aqui despues metemos los codigos que modifican la UI ojala no se me olvide
        //por ahora un debugsito y reiniciamos el juego despues de 2 segundos
        Debug.Log("GAME OVER");
        Invoke(nameof(Reiniciar), 2f);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}