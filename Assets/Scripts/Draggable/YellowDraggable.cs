using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowDraggable : DraggableComponent
{
    public YellowDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.YELLOW];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }
}
