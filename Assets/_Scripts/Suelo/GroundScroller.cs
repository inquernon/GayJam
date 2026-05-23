// GroundScroller.cs — va en cualquier objeto del mundo
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public float zReset = -20f;     // dónde reaparece atrás
    public float zInicio = 40f;     // posición inicial Z del objeto

    void Update()
    {
        transform.position += ScrollManager.Instance.GetDesplazamiento();
        if (transform.position.z < zReset)
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             zInicio);
    }
}