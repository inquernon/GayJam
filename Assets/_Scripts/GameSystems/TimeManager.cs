// Singleton. Gestiona la carga del poder de tiempo.
// El poder se activa/desactiva con un botón en pc (doble toque en móvil).
// Recarga pasiva al recoger relojes — no hay recarga automática.
// Los obstáculos escuchan OnPoderActivado y OnPoderDesactivado.

using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Configuración")]
    public float cargaMaxima = 100f;
    public float gastoPorSegundo = 15f;

    public bool PoderActivo { get; private set; }
    public float CargaNormalizada => cargaActual / cargaMaxima; // 0-1 para la UI

    private float cargaActual;

    public event Action OnPoderActivado;
    public event Action OnPoderDesactivado;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        cargaActual = cargaMaxima;
    }

    void Update()
    {
        if (!PoderActivo) return;

        cargaActual -= gastoPorSegundo * Time.deltaTime;

        if (cargaActual <= 0f)
        {
            cargaActual = 0f;
            DesactivarPoder();
        }
    }

    // Conectar al botón de la UI
    public void ActivarPoder()
    {
        if (PoderActivo || cargaActual <= 0f) return;
        PoderActivo = true;
        OnPoderActivado?.Invoke();
        Debug.Log("poder activado");
    }

    public void DesactivarPoder()
    {
        if (!PoderActivo) return;
        PoderActivo = false;
        OnPoderDesactivado?.Invoke();
        Debug.Log("poder desactivado");
    }

    // Llamado por Reloj.cs
    public void AgregarCarga(float cantidad)
    {
        cargaActual = Mathf.Min(cargaActual + cantidad, cargaMaxima);
    }
}