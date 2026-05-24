// Responsabilidad única: HUD durante el juego.
// Menu y GameOver son escenas separadas — este script no las maneja.
// Requiere un VideoPlayer en el GameObject para la animación del poder.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public Image relojDeArena;    // imagen que se recorta (FillMethod=Vertical, de arriba a abajo)
    public TextMeshProUGUI textoPuntaje;

    [Header("Animación poder")]
    public VideoPlayer videoReloj;      // VideoPlayer con el mp4 del reloj
    public RawImage pantallaVideo;   // RawImage que muestra el video (fullscreen)

    [Header("Slowdown")]
    public float timeScaleActivado = 0.3f;
    public float timeScaleNormal = 1f;

    private bool poderActivo = false;

    void Start()
    {
        // El video no se reproduce al inicio
        pantallaVideo.gameObject.SetActive(false);
        videoReloj.Stop();

        // Suscribirse a eventos del TimeManager
        TimeManager.Instance.OnPoderActivado += OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado += OnPoderDesactivado;
    }

    void Update()
    {
        // Actualiza reloj de arena — se vacía de arriba a abajo
        relojDeArena.fillAmount = TimeManager.Instance.CargaNormalizada;

        // Puntaje
        textoPuntaje.text = Mathf.FloorToInt(ScrollManager.Instance.GetDistancia()) + "m";

        // Cuando el video termina, lo oculta
        if (pantallaVideo.gameObject.activeSelf && !videoReloj.isPlaying)
            pantallaVideo.gameObject.SetActive(false);
    }

    void OnPoderActivado()
    {
        poderActivo = true;
        Time.timeScale = timeScaleActivado;

        // Reproduce el video encima de todo
        pantallaVideo.gameObject.SetActive(true);
        videoReloj.Play();
    }

    void OnPoderDesactivado()
    {
        poderActivo = false;
        Time.timeScale = timeScaleNormal;
        pantallaVideo.gameObject.SetActive(false);
        videoReloj.Stop();
    }

    void OnDestroy()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnPoderActivado -= OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado -= OnPoderDesactivado;
    }
}