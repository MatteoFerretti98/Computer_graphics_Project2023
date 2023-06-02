using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected AnimationAndMovementController pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<AnimationAndMovementController>();
        currentCooldown = weaponData.CooldownDuration; //At the start set the current cooldown to be the cooldown duration   
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f) //Once the cooldown becomes 0, attack
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
