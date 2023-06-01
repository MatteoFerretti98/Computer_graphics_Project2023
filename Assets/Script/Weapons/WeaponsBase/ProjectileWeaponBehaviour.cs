using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script of all projectile behaviours [To be placed on the prefab of a weapon that is a projectile]
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;
    //public float customYPosition; // Y position value to be set

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir, GameObject player)
    {
        direction = dir;
        float dirx = direction.x;
        float dirz = direction.z;
        float playerHeight = player.transform.position.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        // Calculate the angle between the forward direction of the object and the desired direction
        float angle = Mathf.Atan2(dirx, dirz) * Mathf.Rad2Deg;

        // Rotate the object around the Y-axis by the calculated angle
        transform.rotation = Quaternion.Euler(rotation.x, angle, rotation.z);

        // Set Y position of weapon to prefer value to spawn
        Vector3 newPosition = transform.position;
        newPosition.y = playerHeight + 0.5f;
        transform.position = newPosition;

        transform.localScale = scale;
    }
}
