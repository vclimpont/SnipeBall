using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDraggable : DraggableComponent
{
    public OrangeDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.ORANGE];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }

    public override void ActivePower()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Blob"), LayerMask.NameToLayer("BlobWall"), true);
    }

    public override void ResetPower()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Blob"), LayerMask.NameToLayer("BlobWall"), false);
    }
}
