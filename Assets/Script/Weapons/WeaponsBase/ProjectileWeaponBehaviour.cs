using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script of all projectile behaviours [To be placed on the prefab of a weapon that is a projectile]
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    
    protected Vector3 direction;
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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirx = direction.x;
        float dirz = direction.z;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        // Calculate the angle between the forward direction of the object and the desired direction
        float angle = Mathf.Atan2(dirx, dirz) * Mathf.Rad2Deg;

        // Rotate the object around the Y-axis by the calculated angle
        transform.rotation = Quaternion.Euler(rotation.x, angle, rotation.z);

        // Set Y position of weapon to prefer value to spawn
        Vector3 newPosition = transform.position;
        newPosition.y = 1.5f;
        transform.position = newPosition;

        transform.localScale = scale;
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        //Refference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);    // Make sure to use currentDamage instead of weaponData.damage in case any damage multipliers in the future
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            /*if (col.gameObject.TryGetComponent(out BreakableProps breakable))     // DA DECOMMENTARE
            {
                breakable.TakeDamage(currentDamage);
                ReducePierce();
            }*/
        }
    }

    void ReducePierce() //Destroy once the pierce reaches 0
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
