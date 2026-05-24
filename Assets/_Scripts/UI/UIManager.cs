// Maneja toda la UI: menu, HUD, game over.
// SETUP Canvas:
//   Canvas
//   └── UIManager.cs
//       ├── PantallaMenu     (Panel)
//       │   └── BotonJugar   (Button)
//       ├── PantallaHUD      (Panel)
//       │   ├── BarraPoder   (Image, ImageType=Filled, FillMethod=Horizontal)
//       │   └── TextoPuntaje (TextMeshProUGUI)
//       └── PantallaGameOver (Panel)
//           └── TextoGameOver (TextMeshProUGUI)

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject pantallaMenu;
    public GameObject pantallaHUD;
    public GameObject pantallaGameOver;

    [Header("HUD")]
    public Image barraPoder;           // ImageType = Filled
    public TextMeshProUGUI textoPuntaje;

    [Header("Game Over")]
    public TextMeshProUGUI textoGameOver;

    void Start()
    {
        GameManager.Instance.OnEstadoCambiado += ActualizarPantallas;
        ActualizarPantallas(GameManager.Instance.EstadoActual);
    }

    void Update()
    {
        if (GameManager.Instance.EstadoActual != GameManager.Estado.Jugando) return;

        // Barra de poder
        barraPoder.fillAmount = TimeManager.Instance.CargaNormalizada;

        // Puntaje = distancia recorrida
        float distancia = ScrollManager.Instance.GetDistancia();
        textoPuntaje.text = Mathf.FloorToInt(distancia) + "m";
    }

    void ActualizarPantallas(GameManager.Estado estado)
    {
        pantallaMenu.SetActive(estado == GameManager.Estado.Menu);
        pantallaHUD.SetActive(estado == GameManager.Estado.Jugando);
        pantallaGameOver.SetActive(estado == GameManager.Estado.Muerto);

        if (estado == GameManager.Estado.Muerto)
            textoGameOver.text = "GAME OVER\n" + Mathf.FloorToInt(ScrollManager.Instance.GetDistancia()) + "m";
    }

    // Conectar al Button BotonJugar en el inspector (OnClick)
    public void OnBotonJugar()
    {
        GameManager.Instance.IniciarJuego();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnEstadoCambiado -= ActualizarPantallas;
    }
}