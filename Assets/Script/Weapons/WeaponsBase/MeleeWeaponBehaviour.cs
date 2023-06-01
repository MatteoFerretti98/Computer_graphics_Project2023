using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base script of all melee behaviours [To be placed on prefab of a weapon that is melee]
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public float destroyAfterSeconds;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

}
