// Responsabilidad única: HUD durante el juego.
// Menu y GameOver son escenas separadas — este script no las maneja.
// Requiere un VideoPlayer en el GameObject para la animación del poder.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public Image relojDeArena;
    public TextMeshProUGUI textoPuntaje;

    [Header("Animación poder")]
    //public VideoPlayer videoReloj;
    //public RawImage pantallaVideo;

    [Header("Slowdown")]
    public float timeScaleActivado = 0.3f;
    public float timeScaleNormal = 1f;

    [Header("Pausa")]
    public GameObject panelPausa; // Panel con los 3 botones

    private bool poderActivo = false;
    private bool pausado = false;

    void Start()
    {
        //pantallaVideo.gameObject.SetActive(false);
        panelPausa.SetActive(false);
        //videoReloj.Stop();

        TimeManager.Instance.OnPoderActivado += OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado += OnPoderDesactivado;
    }

    void Update()
    {
        if (pausado) return;

        relojDeArena.fillAmount = TimeManager.Instance.CargaNormalizada;
        textoPuntaje.text = Mathf.FloorToInt(ScrollManager.Instance.GetDistancia()) + "m";

        //if (pantallaVideo.gameObject.activeSelf && !videoReloj.isPlaying)
           // pantallaVideo.gameObject.SetActive(false);
    }

    // ── Pausa ─────────────────────────────────────────

    // Conectar al botón de pausa (el || de la pantalla)
    public void OnBotonPausa()
    {
        pausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    // Conectar al botón Continuar dentro del panelPausa
    public void OnBotonContinuar()
    {
        pausado = false;
        Time.timeScale = poderActivo ? timeScaleActivado : timeScaleNormal;
        panelPausa.SetActive(false);
    }

    // Conectar al botón Reiniciar dentro del panelPausa
    public void OnBotonReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Conectar al botón Salir dentro del panelPausa
    public void OnBotonSalir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // escena 0 = Menu
    }

    // ── Poder ─────────────────────────────────────────

    void OnPoderActivado()
    {
        poderActivo = true;
        Time.timeScale = timeScaleActivado;
        //pantallaVideo.gameObject.SetActive(true);
        //videoReloj.Play();
    }

    void OnPoderDesactivado()
    {
        poderActivo = false;
        Time.timeScale = timeScaleNormal;
        //pantallaVideo.gameObject.SetActive(false);
        //videoReloj.Stop();
    }

    void OnDestroy()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnPoderActivado -= OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado -= OnPoderDesactivado;
    }
}