using System.Collections.Generic;
using UnityEngine;

public class ArenaTerrainGenerator : MonoBehaviour
{
    PlayerStats player;

    public int noiseHeight = 2;
    public float detailScale = 8f;

    public GameObject cubePrefab;
    public GameObject[] objectsToSpawn;
    public float spacingBetweenObjects = 1f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerStats>();
        player.transform.position = new Vector3(-30f, 4f, -30f);
        GenerateTerrain();
        SpawnObjectsOnOuterCircle();
    }

    private void GenerateTerrain()
    {
        int radius = 60; // Diametro di 120 diviso per 2

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                // Calcola la coordinata y basata sul noise
                float yNoise = generateNoise(x, z, detailScale) * noiseHeight;

                // Crea un cubo come blocco
                GameObject cube = Instantiate(cubePrefab, transform);

                // Posiziona il cubo in base alle coordinate x, y, z
                cube.transform.position = new Vector3(x, yNoise, z);
            }
        }
    }

    private void SpawnObjectsOnOuterCircle()
    {
        int radius = 60; // Diametro di 120 diviso per 2
        int outerRadius = radius - 3; // Distanza di 2 o 3 blocchi dalla circonferenza esterna

        // Calcola il numero di oggetti da spawnare
        int numObjects = Mathf.FloorToInt(2 * Mathf.PI * outerRadius / spacingBetweenObjects);

        // Calcola l'angolo tra gli oggetti
        float angleStep = 2f * Mathf.PI / numObjects;

        for (int i = 0; i < numObjects; i++)
        {
            // Calcola l'angolo di posizione dell'oggetto
            float angle = i * angleStep;

            // Calcola le coordinate x e z sull'anello esterno
            float x = outerRadius * Mathf.Cos(angle);
            float z = outerRadius * Mathf.Sin(angle);

            // Seleziona un oggetto casuale dalla lista degli oggetti da spawnare
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            // Calcola l'offset di spawn
            float yOffset = GetSpawnOffset();

            // Crea l'oggetto
            SpawnObject(objectToSpawn, new Vector3(x, yOffset, z));
        }
    }

    private void SpawnObject(GameObject objectToSpawn, Vector3 position)
    {
        Instantiate(objectToSpawn, transform.position + position, Quaternion.identity);
    }

    private float GetSpawnOffset()
    {
        return ShouldApplyYAxisRestrictions() ? 1.5f : 0.5f;
    }

    private bool ShouldApplyYAxisRestrictions()
    {
        return transform.position.y <= 2.0f && transform.position.y >= 1.5f;
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + transform.position.x) / detailScale;
        float zNoise = (z + transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
