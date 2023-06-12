using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostItemScriptableObject", menuName = "ScriptableObjects/Boost Item")]
public class BoostItemScriptableObject : ScriptableObject
{
    [SerializeField]
    int increment;
    public int Increment { get => increment; private set => increment = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;    //What is the description of this weapon? [If this weapon is an upgrade, place the description of the upgrades]
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;    //Not meant to be modified in game [Only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }
}