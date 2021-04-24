using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDraggable : DraggableComponent
{
    public OrangeDraggable(PlayerController _pc) : base(_pc)
    {

    }

    public override void ActivePower()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Blob"), LayerMask.NameToLayer("BlobWall"), true);
    }

    public override void ResetPower()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Blob"), LayerMask.NameToLayer("BlobWall"), false);
    }
}
