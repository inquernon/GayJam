// test de objeto Árbol caído: ocupa los 3 carriles en estado presente.
// Con el poder: se vuelve árbol normal a un lado, los 3 carriles quedan libres.
// SETUP del prefab:
//   Arbol (GameObject)
//   ├── Arbol.cs
//   ├── Collider (IsTrigger = true, cubre los 3 carriles en estado presente)
//   ├── MeshPresente  (árbol caído atravesando el camino)
//   └── MeshPasado    (árbol erguido pequeño, sin collider o collider desactivado)

using UnityEngine;

public class Arbol : ObstaculoBase
{
    public GameObject meshPresente;
    public GameObject meshPasado;
    public Collider coliderPresente; // el collider bloqueante

    void Awake()
    {
        carrilesPresente = 3;
        carrilesPasado = 0; // no bloquea ningún carril
    }

    protected override void EstadoPresente()
    {
        enEstadoPasado = false; 
        
        if (meshPresente == null) { Debug.LogError("meshPresente no asignado en " + gameObject.name); return; }
        if (meshPasado == null) { Debug.LogError("meshPasado no asignado en " + gameObject.name); return; }


        meshPresente.SetActive(true);
        meshPasado.SetActive(false);
        coliderPresente.enabled = true;
    }

    protected override void EstadoPasado()
    {
        enEstadoPasado = true;

        if (meshPresente == null) { Debug.LogError("meshPresente no asignado en " + gameObject.name); return; }
        if (meshPasado == null) { Debug.LogError("meshPasado no asignado en " + gameObject.name); return; }

        meshPresente.SetActive(false);
        meshPasado.SetActive(true);
        coliderPresente.enabled = false;
    }

}