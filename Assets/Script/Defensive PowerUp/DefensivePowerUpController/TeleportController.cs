using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : DefensivePowerUpController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Defend()
    {
        base.Defend();

        if (!GameManager.instance.BossFightTime)
        {
            GameObject spawnedTeleportEffect = Instantiate(defensivePowerUpData.Prefab);
            spawnedTeleportEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); //Assign the position to be the same as this object which is parented to the player
            spawnedTeleportEffect.transform.parent = transform; // So that is spawns below this object
            Teleport(); // Teletrasporto
        }
    }

    // Metodo per il teletrasporto
    protected void Teleport()
    {
        Vector3 playerPosition = transform.position;
        Vector3 teleportPosition = new Vector3(
                playerPosition.x + Random.Range(-20f, 20f),
                playerPosition.y + 1f,
                playerPosition.z + Random.Range(-20f, 20f)
            );
        player.transform.position = teleportPosition;
    }
}
