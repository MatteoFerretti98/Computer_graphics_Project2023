using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationDefensivePowerUpBehaviour : MonoBehaviour
{
    public DefensivePowerUpScriptableObject defensivePowerUpData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Statistiche correnti
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
