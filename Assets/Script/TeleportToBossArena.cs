using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToBossArena : SceneController
{

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            SceneChange("BossArena");
        }
    }
}
