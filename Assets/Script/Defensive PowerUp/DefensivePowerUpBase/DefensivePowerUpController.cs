using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePowerUpController : MonoBehaviour
{
    [Header("Defensive PowerUp Stats")]
    public DefensivePowerUpScriptableObject defensivePowerUpData;
    float currentCooldown;
    protected PlayerStats player;

    protected AnimationAndMovementController pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<AnimationAndMovementController>();
        currentCooldown = defensivePowerUpData.CooldownDuration; //At the start set the current cooldown to be the cooldown duration   
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) //Once the cooldown becomes 0, defend
        {
            Defend();
        }
    }

    protected virtual void Defend()
    {
        currentCooldown = defensivePowerUpData.CooldownDuration;
    }

    protected virtual void ApplyModifier()
    {
        // Apply the boost value to the appropriate player stat in the child classes
    }
}
