using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollectible
{
    public int experienceGranted;
    public GameObject expEffect;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        Destroy(gameObject);

        // Spawn del prefab
        GameObject spawnedPrefab = Instantiate(expEffect, transform.position, Quaternion.identity);

        // Scomparsa del prefab dopo 2 secondi
        Destroy(spawnedPrefab, 2f);
    }
}
