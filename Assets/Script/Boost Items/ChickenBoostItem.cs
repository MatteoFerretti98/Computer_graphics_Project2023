using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBoostItem : BoostItem
{
    protected override void ApplyModifier()
    {
        Debug.Log("aumento vita");
        player.CurrentHealth += boostItemData.Increment;
    }
}
