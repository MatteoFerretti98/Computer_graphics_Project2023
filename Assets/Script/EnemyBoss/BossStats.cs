using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStats : MonoBehaviour
{
    public BossScriptableObject bossData;

    //Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage1;
    [HideInInspector]
    public float currentDamage2;
    [HideInInspector]
    public float currentDamage3;

    Transform player;

    public Image bossHealthBar;

    void Awake()
    {
        //Assign the vaiables
        currentMoveSpeed = bossData.MoveSpeed;
        currentHealth = bossData.MaxHealth;
        currentDamage1 = bossData.Damage1;
        currentDamage2 = bossData.Damage2;
        currentDamage3 = bossData.Damage3;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        bossHealthBar = GameObject.Find("/Canvas/Screens/Game Screen/Boss Health Bar Holder/Boss Health Bar").GetComponent<Image>();

        UpdateBossHealthBar();
    }

    void Update()
    {
        UpdateBossHealthBar();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Kill();
        }
        UpdateBossHealthBar();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void UpdateBossHealthBar()
    {
        bossHealthBar.fillAmount = currentHealth / bossData.MaxHealth;
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage1);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }

    void OnCollisionStay(Collision col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage1);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }
}
