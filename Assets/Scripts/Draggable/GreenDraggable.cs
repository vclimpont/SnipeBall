using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDraggable : DraggableComponent
{
    public GreenDraggable(PlayerController _pc) : base(_pc)
    {

    }

    public override void ActivePower()
    {
        GameManager.gravityMultiplier = 0.25f;
    }

    public override void ResetPower()
    {
        GameManager.gravityMultiplier = 1f;
    }
}
