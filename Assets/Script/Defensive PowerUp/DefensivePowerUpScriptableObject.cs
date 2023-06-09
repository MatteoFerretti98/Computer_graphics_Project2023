using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefensivePowerUpScriptableObject", menuName = "ScriptableObjects/DefensivePowerUp")]
public class DefensivePowerUpScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    [SerializeField]
    float duration;
    public float Duration { get => duration; private set => duration = value; }

    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    int level;  //Not meant to be modified in game [Only in Editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;  //The prefab of the next level i.e. what the object becomes when it levels up
                                 //Not to be confused with the prefab to be spawned at the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite icon;    //Not meant to be modified in game [Only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }
}
