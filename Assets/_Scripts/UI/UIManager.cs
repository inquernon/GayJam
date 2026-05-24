// Responsabilidad única: HUD durante el juego.
// Menu y GameOver son escenas separadas — este script no las maneja.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public Image relojDeArena;
    public TextMeshProUGUI textoPuntaje;

    [Header("Slowdown")]
    public float timeScaleActivado = 0.3f;
    public float timeScaleNormal = 1f;

    [Header("Pausa")]
    public GameObject panelPausa;

    [Header("Game Over")]
    public GameObject panelGameOver;

    private bool poderActivo = false;
    private bool pausado = false;

    void Start()
    {
        panelPausa.SetActive(false);

        TimeManager.Instance.OnPoderActivado += OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado += OnPoderDesactivado;

        GameManager.Instance.OnEstadoCambiado += estado =>
        {
            if (estado == GameManager.Estado.Muerto)
                GameOver();
        };
    }

    void Update()
    {
        if (pausado) return;

        relojDeArena.fillAmount = TimeManager.Instance.CargaNormalizada;
        textoPuntaje.text = Mathf.FloorToInt(ScrollManager.Instance.GetDistancia()) + "m";
    }

    // ── Pausa ─────────────────────────────────────────

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    public void Continuar()
    {
        pausado = false;
        Time.timeScale = poderActivo ? timeScaleActivado : timeScaleNormal;
        panelPausa.SetActive(false);
    }

    public void OnBotonPausa()
    {
        pausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
    }

    public void OnBotonContinuar()
    {
        pausado = false;
        Time.timeScale = poderActivo ? timeScaleActivado : timeScaleNormal;
        panelPausa.SetActive(false);
    }

    public void OnBotonReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBotonSalir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // ── Poder ─────────────────────────────────────────

    void OnPoderActivado()
    {
        poderActivo = true;
        Time.timeScale = timeScaleActivado;
    }

    void OnPoderDesactivado()
    {
        poderActivo = false;
        Time.timeScale = timeScaleNormal;
    }

    void OnDestroy()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnPoderActivado -= OnPoderActivado;
        TimeManager.Instance.OnPoderDesactivado -= OnPoderDesactivado;
    }
}
