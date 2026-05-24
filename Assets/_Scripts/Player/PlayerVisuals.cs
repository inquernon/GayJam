// PlayerVisuals.cs
// Cambia el modelo del personaje cuando el poder se activa/desactiva.
// SETUP:
//   Player
//   ├── PlayerVisuals.cs
//   ├── ModeloAdulto  (GameObject hijo, activo por defecto)
//   └── ModeloNino    (GameObject hijo, inactivo por defecto)

using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public GameObject modeloAdulto;
    public GameObject modeloNino;

    void Start()
    {
        TimeManager.Instance.OnPoderActivado += MostrarNino;
        TimeManager.Instance.OnPoderDesactivado += MostrarAdulto;
        MostrarAdulto(); // estado inicial
    }

    void MostrarAdulto()
    {
        modeloAdulto.SetActive(true);
        modeloNino.SetActive(false);
    }

    void MostrarNino()
    {
        modeloAdulto.SetActive(false);
        modeloNino.SetActive(true);
    }

    void OnDestroy()
    {
        if (TimeManager.Instance == null) return;
        TimeManager.Instance.OnPoderActivado -= MostrarNino;
        TimeManager.Instance.OnPoderDesactivado -= MostrarAdulto;
    }
}