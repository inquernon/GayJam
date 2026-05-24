using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Efecto Flotación (Seno)")]
    public float amplitud = 0.3f;
    public float velocidadFlotacion = 3f;

    [Header("Efecto Rotación (Trompo en Y)")]
    public float velocidadRotacionY = 180f;

    private Vector3 centroGeometricoLocal;
    private float posicionYInicial;
    private float anguloYActual;

    void Start()
    {
        // 1. Obtenemos el componente que dibuja la malla
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Calculamos la distancia exacta entre el pivote malo (transform.position) 
            // y el centro real del modelo (meshRenderer.bounds.center)
            Vector3 centroMundial = meshRenderer.bounds.center;
            centroGeometricoLocal = transform.InverseTransformPoint(centroMundial);
        }
        else
        {
            // Si no hay malla, asumimos el centro en cero (por si acaso)
            centroGeometricoLocal = Vector3.zero;
        }

        // 2. Guardamos la altura inicial
        posicionYInicial = transform.position.y;
        anguloYActual = transform.localEulerAngles.y;
    }

    void Update()
    {
        // 3. FLOTACIÓN: Calculamos la altura Y exacta en este frame
        float nuevaY = posicionYInicial + Mathf.Sin(Time.time * velocidadFlotacion) * amplitud;

        // 4. ROTACIÓN: Calculamos el nuevo ángulo del trompo
        anguloYActual += velocidadRotacionY * Time.deltaTime;
        Quaternion nuevaRotacion = Quaternion.Euler(0f, anguloYActual, 0f);

        // 5. TRUCO MATEMÁTICO (Compensación de Pivote Malo):
        // Convertimos el desfase del centro a coordenadas del mundo basándonos en la nueva rotación
        Vector3 desfaseMundial = nuevaRotacion * centroGeometricoLocal;

        // Aplicamos la posición final restando el desfase para que el objeto 
        // no se mueva de su sitio mientras el pivote orbita por detrás
        transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z) - desfaseMundial + (transform.rotation * centroGeometricoLocal);

        // Aplicamos la rotación limpia
        transform.localRotation = nuevaRotacion;
    }
}