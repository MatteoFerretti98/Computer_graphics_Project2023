using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, ICollectible
{
    // The amount of health to restore when this item is collected
    public int healthToRestore;

    public GameObject heartEffect;

    public void Collect()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        stats.RestoreHealth(healthToRestore);
        Destroy(gameObject);

        // Spawn del prefab
        GameObject spawnedPrefab = Instantiate(heartEffect, transform.position, Quaternion.identity);

        // Scomparsa del prefab dopo 2 secondi
        Destroy(spawnedPrefab, 2f);
    }
}
