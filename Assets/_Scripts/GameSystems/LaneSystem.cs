using UnityEngine;

public class LaneSystem : MonoBehaviour
{
    public static LaneSystem Instance { get; private set; }

    public float separacion = 2.5f;  // distancia entre carriles

    // Carril: -1 = izquierda, 0 = centro, 1 = derecha
    public Vector3 GetPosicionCarril(int carril)
    {
        return new Vector3(carril * separacion, 0f, 0f);
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }
}