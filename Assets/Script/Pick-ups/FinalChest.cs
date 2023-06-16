using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChest : MonoBehaviour, ICollectible
{
    // The amount of money to add when this item is collected
    public int CoinToAdd;

    public GameObject coinEffect; // Prefab da far apparire
    public int numPrefabsToSpawn = 10;
    public float spawnRadius = 1f;
    public float delayBetweenSpawns = 0.5f;

    public void Collect()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        stats.IncrementCoins(CoinToAdd);
        Destroy(gameObject);

        StartCoroutine(SpawnPrefabsWithDelay());

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.ChangeState(GameManager.GameState.Win);
    }

    IEnumerator SpawnPrefabsWithDelay()
    {
        for (int i = 0; i < numPrefabsToSpawn; i++)
        {
            // Calcola una posizione casuale all'interno dell'intervallo
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;

            // Applica l'offset rispetto alla posizione della cassa finale
            Vector3 spawnPosition = new Vector3(transform.position.x + randomOffset.x, transform.position.y + 0.5f, transform.position.z + randomOffset.z);

            // Spawn del prefab
            GameObject spawnedPrefab = Instantiate(coinEffect, spawnPosition, Quaternion.identity);

            // Scomparsa del prefab dopo 2 secondi
            Destroy(spawnedPrefab, 2f);

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
