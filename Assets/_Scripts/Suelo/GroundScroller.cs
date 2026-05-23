// GroundScroller.cs — va en cualquier objeto del mundo
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public float zReset = -20f;     // dónde reaparece atrás
    public float zInicio = 40f;     // posición inicial Z del objeto
    public float L = 20f;           // longitud del objeto
    public int N = 5;              // cuántas veces se repite el objeto

    void Update()
    {
        zInicio = zReset + (L * N);
        transform.position += ScrollManager.Instance.GetDesplazamiento();
        if (transform.position.z < zReset)
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             zInicio);
    }
}