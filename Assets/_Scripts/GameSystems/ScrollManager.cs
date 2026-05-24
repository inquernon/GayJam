using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public static ScrollManager Instance { get; private set; }

    [Header("Velocidad")]
    public float velocidadInicial = 8f;
    public float velocidadActual { get; private set; }

    private float distanciaRecorrida = 0f;
    [SerializeField] private bool corriendo = true;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        velocidadActual = velocidadInicial;
    }

    void Update()
    {
        if (!corriendo) return;
        // unscaledDeltaTime ignora timeScale — la distancia sigue contando normal
        distanciaRecorrida += velocidadActual * Time.unscaledDeltaTime;
    }

    // Todo objeto del mundo llama esto para moverse
    public Vector3 GetDesplazamiento()
    {
        return Vector3.back * velocidadActual * Time.deltaTime;
    }

    public float GetDistancia() => distanciaRecorrida;
    public void Iniciar() => corriendo = true;
    public void Detener() => corriendo = false;
    public void SetVelocidad(float v) => velocidadActual = v;
}