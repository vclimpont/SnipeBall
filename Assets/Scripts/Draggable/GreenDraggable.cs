using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDraggable : DraggableComponent
{
    public GreenDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.GREEN];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }

    public override void ActivePower()
    {
        GameManager.gravityMultiplier = 0f;
        GameManager.dragMultiplier = 2.5f;
    }

    public override void ResetPower()
    {
        GameManager.gravityMultiplier = 1f;
        GameManager.dragMultiplier = 0f;
    }
}
