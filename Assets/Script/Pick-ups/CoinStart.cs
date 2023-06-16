using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStar : MonoBehaviour, ICollectible
{
    // The amount of money to add when this item is collected
    public int CoinToAdd;

    public GameObject coinEffect; // Prefab da far apparire

    public void Collect()
    {
        AudioManager.instance.PlaySFX("StarCollect");
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        stats.IncrementCoins(CoinToAdd);
        Destroy(gameObject);

        // Spawn del prefab
        GameObject spawnedPrefab = Instantiate(coinEffect, transform.position, Quaternion.identity);

        // Scomparsa del prefab dopo 2 secondi
        Destroy(spawnedPrefab, 2f);
    }
}
