using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    void Awake()
    {
        //Assign the vaiables
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {   
        if (Vector3.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
        
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.LogWarning("damage:"+dmg);
        if (currentHealth <= 0)
        {
            Debug.LogWarning("Death Monster");
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }

    void OnCollisionStay(Collision col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }

    private void OnDestroy()
    {
        try
        {
            EnemySpawner es = FindObjectOfType<EnemySpawner>();
            es.OnEnemyKilled();
        }
        catch (Exception e)
        {
            // Scrivere eventualmente stringa di errore
        }
        
        
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[UnityEngine.Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }
}
