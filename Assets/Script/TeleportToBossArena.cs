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
            player = FindAnyObjectByType<PlayerStats>();

            SceneChange("BossArena");
        }
    }

}
