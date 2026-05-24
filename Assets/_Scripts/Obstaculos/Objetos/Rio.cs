// Rio.cs
// Presente: río ancho, ocupa 3 carriles, no se puede pasar.
// Pasado:   río pequeño, ocupa 3 carriles PERO se puede saltar (collider desactivado).
// DIFERENCIA vs Arbol: en estado pasado el mesh sigue visible pero el collider
// se desactiva — el jugador lo salta visualmente.
// SETUP prefab:
//   Rio (raíz)
//   ├── Rio.cs
//   ├── Box Collider (IsTrigger=true, 3 carriles de ancho, altura baja)
//   ├── MeshPresente  (río ancho, activo)
//   └── MeshPasado    (arroyo pequeño, inactivo)

using UnityEngine;

public class Rio : ObstaculoBase
{
    public GameObject meshPresente;
    public GameObject meshPasado;
    public Collider coliderPresente;

    void Awake()
    {
        carrilesPresente = 3;
        carrilesPasado = 3; // sigue visible pero jumpeable
    }

    protected override void EstadoPresente()
    {
        enEstadoPasado = false;
        meshPresente.SetActive(true);
        meshPasado.SetActive(false);
        coliderPresente.enabled = true;
    }

    protected override void EstadoPasado()
    {
        enEstadoPasado = true;
        meshPresente.SetActive(false);
        meshPasado.SetActive(true);
        coliderPresente.enabled = false; // jumpeable, no mata
    }
}