using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Carriles")]
    private int carrilActual = 0;
    private float xObjetivo = 0f;
    public float velocidadLateral = 10f;

    [Header("Salto")]
    public float fuerzaSalto = 12f;
    public float gravedad = -25f;
    private float velocidadY = 0f;
    private bool enSuelo = true;

    [Header("Slide")]
    public float duracionSlide = 0.6f;
    private bool slidingActivo = false;

    [Header("Colisión")]
    public float alturaColisionNormal = 2f;
    public float alturaColisionSlide = 1f;
    private CharacterController cc;

    // Input swipe móvil
    private Vector2 touchInicio;
    private bool swipeRegistrado = false;

    private Vector3 _movimientoFrame;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!ScrollManager.Instance) return;

        _movimientoFrame = Vector3.zero;
        ProcesarInput();
        AplicarMovimientoLateral();
        AplicarGravedad();
        cc.Move(_movimientoFrame);
    }

    // ── Input ─────────────────────────────────────────

    void ProcesarInput()
    {
#if UNITY_EDITOR
        // Teclado para testing rápido
        if (Input.GetKeyDown(KeyCode.A)) CambiarCarril(-1);
        if (Input.GetKeyDown(KeyCode.D)) CambiarCarril(1);
        if (Input.GetKeyDown(KeyCode.W)) Saltar();
        if (Input.GetKeyDown(KeyCode.S)) IniciarSlide();
        if (Input.GetKeyDown(KeyCode.Space)) Saltar();
        if (Input.GetKeyDown(KeyCode.T)) TimeManager.Instance.TogglePoder();
#endif
        ProcesarSwipe();
    }

    void ProcesarSwipe()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
        {
            touchInicio = t.position;
            swipeRegistrado = false;
        }

        if (t.phase == TouchPhase.Moved && !swipeRegistrado)
        {
            Vector2 delta = t.position - touchInicio;

            if (delta.magnitude < 30f) return; // umbral mínimo

            swipeRegistrado = true;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                CambiarCarril(delta.x > 0 ? 1 : -1);
            }
            else
            {
                if (delta.y > 0) Saltar();
                else IniciarSlide();
            }
        }
    }

    // ── Movimiento ────────────────────────────────────

    void CambiarCarril(int direccion)
    {
        int nuevoCarril = Mathf.Clamp(carrilActual + direccion, -1, 1);
        if (nuevoCarril == carrilActual) return;

        carrilActual = nuevoCarril;
        xObjetivo = carrilActual * 2.5f;
    }

    void AplicarMovimientoLateral()
    {
        float xActual = transform.position.x;
        float xNuevo = Mathf.MoveTowards(xActual, xObjetivo, velocidadLateral * Time.deltaTime);
        _movimientoFrame.x = xNuevo - xActual;
    }

    void AplicarGravedad()
    {
        if (cc.isGrounded)
        {
            enSuelo = true;
            if (velocidadY < 0f) velocidadY = -2f;
        }
        else
        {
            enSuelo = false;
        }

        velocidadY += gravedad * Time.deltaTime;
        _movimientoFrame.y = velocidadY * Time.deltaTime;
    }

    void Saltar()
    {
        if (!enSuelo) return;
        velocidadY = fuerzaSalto;
        enSuelo = false;
    }

    void IniciarSlide()
    {
        if (slidingActivo) return;
        StartCoroutine(SlideCoroutine());
    }

    IEnumerator SlideCoroutine()
    {
        slidingActivo = true;
        cc.height = alturaColisionSlide;

        yield return new WaitForSeconds(duracionSlide);

        cc.height = alturaColisionNormal;
        slidingActivo = false;
    }

    // ── API pública ───────────────────────────────────

    public bool EstaEnSuelo() => enSuelo;
    public bool EstaSliding() => slidingActivo;
    public int GetCarril() => carrilActual;
}