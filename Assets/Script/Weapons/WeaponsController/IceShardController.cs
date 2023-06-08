using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardController : WeaponController
{
    float launchAngle = 45f;

    protected override void Attack()
    {
        base.Attack();

        // Calculate the direction for projectile launch
        Vector3 playerForward = pm.transform.forward;

        Vector3 launchDirection1 = Quaternion.Euler(0f, launchAngle, 0f) * playerForward;
        Vector3 launchDirection2 = Quaternion.Euler(0f, -launchAngle, 0f) * playerForward;

        // Instantiate two copies of the projectile and set their direction
        GameObject projectile1 = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
        projectile1.GetComponent<IceShardBehaviour>().DirectionChecker(launchDirection1, transform.position);

        GameObject projectile2 = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
        projectile2.GetComponent<IceShardBehaviour>().DirectionChecker(launchDirection2, transform.position);
    }
}
