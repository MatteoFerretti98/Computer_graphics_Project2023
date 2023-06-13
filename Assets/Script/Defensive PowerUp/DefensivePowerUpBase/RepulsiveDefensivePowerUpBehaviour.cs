using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsiveDefensivePowerUpBehaviour : MonoBehaviour
{
    public DefensivePowerUpScriptableObject defensivePowerUpData;

    public float destroyAfterSeconds;

    // Current Stats
    protected float currentDuration;
    protected float currentCooldownDuration;
    protected float currentSpeed;


    void Awake()
    {
        currentDuration = defensivePowerUpData.Duration;
        currentSpeed = defensivePowerUpData.Speed;
        currentCooldownDuration = defensivePowerUpData.CooldownDuration;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}
