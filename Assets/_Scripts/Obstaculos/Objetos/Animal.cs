// Animal.cs
// Presente: animal adulto, ocupa 2 carriles.
// Pasado:   cría, ocupa 1 carril (se hace a un lado).
// El collider cambia de tamaño entre estados.
// SETUP prefab:
//   Animal (raíz)
//   ├── Animal.cs
//   ├── Box Collider (IsTrigger=true) ← coliderPresente, 2 carriles
//   ├── ColiderPasado (GameObject hijo con Box Collider, 1 carril) ← se activa en pasado
//   ├── MeshPresente  (adulto, activo)
//   └── MeshPasado    (cría, inactivo)

using UnityEngine;

public class Animal : ObstaculoBase
{
    public GameObject meshPresente;
    public GameObject meshPasado;
    public Collider coliderPresente; // 2 carriles
    public Collider coliderPasado;   // 1 carril — tiene su propio OnTriggerEnter

    void Awake()
    {
        carrilesPresente = 2;
        carrilesPasado = 1;
    }

    protected override void EstadoPresente()
    {
        enEstadoPasado = false;
        meshPresente.SetActive(true);
        meshPasado.SetActive(false);
        coliderPresente.enabled = true;
        coliderPasado.enabled = false;
    }

    protected override void EstadoPasado()
    {
        enEstadoPasado = true;
        meshPresente.SetActive(false);
        meshPasado.SetActive(true);
        coliderPresente.enabled = false;
        coliderPasado.enabled = true; // la cría sigue ocupando 1 carril
    }

    // Override: en estado pasado la cría sigue matando si chocas con ella
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // En cualquier estado mata — la cría sigue siendo obstáculo
        GameManager.Instance.PlayerMurio();
    }
}