using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvicibilityCircleController : DefensivePowerUpController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Defend()
    {
        base.Defend();
        GameObject spawnedInvincibilityCircle = Instantiate(defensivePowerUpData.Prefab);
        spawnedInvincibilityCircle.transform.position = transform.position; //Assign the position to be the same as this object which is parented to the player
        spawnedInvincibilityCircle.transform.parent = transform; // So that is spawns below this object
    }

    protected override void ApplyModifier()
    {
        player.invincibilityDuration *= 1 + defensivePowerUpData.Duration / 100f;
    }
}
