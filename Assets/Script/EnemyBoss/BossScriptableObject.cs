using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossScriptableObject", menuName = "ScriptableObjects/Boss")]
public class BossScriptableObject : ScriptableObject
{
    //Base stats for the Boss
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    float damage1;
    public float Damage1 { get => damage1; private set => damage1 = value; }

    [SerializeField]
    float damage2;
    public float Damage2 { get => damage2; private set => damage2 = value; }

    [SerializeField]
    float damage3;
    public float Damage3 { get => damage3; private set => damage3 = value; }
}
