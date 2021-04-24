using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDraggable : DraggableComponent
{
    public BlueDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.BLUE];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }
}
