using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItem : MonoBehaviour
{
    protected PlayerStats player;
    public BoostItemScriptableObject boostItemData;

    protected virtual void ApplyModifier()
    {
        // Apply the boost value to the appropriate player stat in the child classes
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
}
