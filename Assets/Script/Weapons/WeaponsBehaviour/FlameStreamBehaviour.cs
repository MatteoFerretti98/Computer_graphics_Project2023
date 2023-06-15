using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStreamBehaviour : SpitAreaWeaponBehavior
{
    List<GameObject> markedEnemies;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if ((col.CompareTag("Enemy") || col.CompareTag("EnemyBoss")) && !markedEnemies.Contains(col.gameObject))
        {
            if (col.CompareTag("Enemy")) 
            {
                EnemyStats enemy = col.GetComponent<EnemyStats>();
                enemy.TakeDamage(GetCurrentDamage());
            }
            else
            {
                BossStats boss = col.GetComponent<BossStats>();
                boss.TakeDamage(GetCurrentDamage());
            }
            markedEnemies.Add(col.gameObject); // Mark the enemy so it doesn't take another instance of damage from this garlic
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(GetCurrentDamage());

                markedEnemies.Add(col.gameObject);
            }
        }
    }
}
