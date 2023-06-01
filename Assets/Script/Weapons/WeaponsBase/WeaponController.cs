using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [Header("Weapon Stats")]
    public GameObject prefab;
    public GameObject player;
    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce;

    protected AnimationAndMovementController pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<AnimationAndMovementController>();
        currentCooldown = cooldownDuration; //At the start set the current cooldown to be the cooldown duration   
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
        currentCooldown = cooldownDuration;
    }
}
