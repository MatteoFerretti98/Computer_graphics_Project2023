using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToBossArena : SceneController
{
    PlayerStats player;

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            SceneChange("BossArena");

            player = FindAnyObjectByType<PlayerStats>();

            player.transform.position = new Vector3(0, 4f, 0);
        }
    }
}
