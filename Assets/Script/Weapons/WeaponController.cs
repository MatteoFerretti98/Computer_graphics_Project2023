using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = cooldownDuration; //At the start set the current cooldown to be the cooldown duration   
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f) //Once the cooldown becomes 0, attack
        {
            Attack();
        }
    }

    void Attack()
    {
        currentCooldown = cooldownDuration;
    }
}
