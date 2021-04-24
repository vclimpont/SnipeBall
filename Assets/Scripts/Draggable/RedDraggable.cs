using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDraggable : DraggableComponent
{
    public RedDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.RED];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }


}
