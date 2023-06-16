using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBlast : MonoBehaviour
{
    BossStats bossStats;

    // Start is called before the first frame update
    void Start()
    {
        bossStats = FindObjectOfType<BossStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(bossStats.currentDamage2);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }

    void OnCollisionStay(Collision col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(bossStats.currentDamage2);    //Make sure to use currentDamage instead of weaponData.Damage in case any damage multipliers in the future
        }
    }
}
