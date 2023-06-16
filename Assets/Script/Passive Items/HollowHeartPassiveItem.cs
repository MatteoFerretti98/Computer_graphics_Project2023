using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowHeartPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.MaxHealth *= 1 + passiveItemData.Multiplier / 100f;
    }
}
