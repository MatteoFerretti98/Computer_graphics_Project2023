using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBoostItem : BoostItem
{
    protected override void ApplyModifier()
    {
        Debug.Log("Aumento soldi");
        player.CurrentCoins += boostItemData.Increment;
    }
}
