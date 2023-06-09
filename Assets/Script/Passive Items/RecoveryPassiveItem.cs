using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentRecovery *= 1 + passiveItemData.Multiplier / 100f;
    }
}
