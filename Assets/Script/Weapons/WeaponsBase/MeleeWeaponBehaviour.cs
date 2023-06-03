using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base script of all melee behaviours [To be placed on prefab of a weapon that is melee]
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    // Current Stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Refference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());    // Make sure to use currentDamage instead of weaponData.damage in case any damage multipliers in the future
        }
        else if (col.CompareTag("Prop"))
        {
            /*if (col.gameObject.TryGetComponent(out BreakableProps breakable))         // DA DECOMMENTARE
            {
                breakable.TakeDamage(GetCurrentDamage());
            }*/
        }
    }

}
