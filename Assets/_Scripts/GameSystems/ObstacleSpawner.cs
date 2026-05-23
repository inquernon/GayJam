// Spawnea obstáculos adelante del jugador en carriles válidos.
// Cada obstáculo define cuántos carriles ocupa (carrilesPresente).
// El spawner elige un carril de origen válido para que el obstáculo
// no quede fuera del camino.
//
// Lógica de carriles por tamaño:
//   ocupa 1 carril → puede spawnear en -1, 0 o 1
//   ocupa 2 carriles → puede spawnear en -1 o 0 (ocupa origen y origen+1)
//   osea un carril extra a la derecha del origen, por eso no puede spawnear en 1 
//   ocupa 3 carriles → siempre spawnea en 0 (centrado)
//
// SETUP: arrastrar los prefabs de obstáculos al array en el inspector
// (cuidado con la referencia a ObstaculoBase, debe estar en el mismo prefab) !!

using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject[] obstaculosPrefabs;

    [Header("Configuración")]
    public float intervaloSpawn = 2.5f;
    public float distanciaSpawn = 80f;   // Z adelante del jugador
    public float separacionCarriles = 2.5f;
    // Agrega referencia al jugador
    public Transform jugador;


    private float timer = 0f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervaloSpawn)
        {
            timer = 0f;
            Spawner();
        }
    }

    void Spawner()
    {
        if (obstaculosPrefabs.Length == 0) return;

        GameObject prefab = obstaculosPrefabs[Random.Range(0, obstaculosPrefabs.Length)];
        ObstaculoBase datos = prefab.GetComponent<ObstaculoBase>();

        float x;
        if (datos == null)
        {
            // Es un pickup (Reloj), spawn en carril aleatorio simple
            int carril = Random.Range(-1, 2);
            x = carril * separacionCarriles;
        }
        else
        {
            int carrilOrigen = GetCarrilOrigen(datos.carrilesPresente);
            x = GetPosicionX(carrilOrigen, datos.carrilesPresente);
        }

        Vector3 posicion = new Vector3(x, 0f, jugador.position.z + distanciaSpawn);
        Instantiate(prefab, posicion, Quaternion.identity);
    }

    int GetCarrilOrigen(int carrilesQueOcupa)
    {
        switch (carrilesQueOcupa)
        {
            case 1:
                // Cualquier carril: -1, 0 o 1
                return Random.Range(-1, 2);
            case 2:
                // Solo -1 o 0 para que no se salga del camino
                return Random.Range(-1, 1);
            case 3:
                // Siempre centrado
                return 0;
            default:
                return 0;
        }
    }

    float GetPosicionX(int carrilOrigen, int carrilesQueOcupa)
    {
        switch (carrilesQueOcupa)
        {
            case 1:
                return carrilOrigen * separacionCarriles;
            case 2:
                // Centro entre carrilOrigen y carrilOrigen+1
                return (carrilOrigen + 0.5f) * separacionCarriles;
            case 3:
                // Centrado en X = 0
                return 0f;
            default:
                return 0f;
        }
    }

    // Llamado por DifficultyManager cuando exista si no cambio la logica mas adelante
    public void SetIntervalo(float intervalo)
    {
        intervaloSpawn = Mathf.Max(intervalo, 0.5f);
    }
}