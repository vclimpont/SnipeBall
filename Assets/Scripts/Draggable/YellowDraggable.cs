using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowDraggable : DraggableComponent
{
    private float currentScaleMultiplier;

    public YellowDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.YELLOW];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;
    }

    public override void OnDragMoved(GameObject currentBlob, bool mouse = false)
    {
        if (!pc.dragStarted) return;

        //Move blob according to current touch position
        Vector3 currentDragPosition = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        currentDragPosition.z = 0;

        Vector3 localDragPosition = currentDragPosition + (pc.transform.position - pc.dragStartPos);
        line.SetPosition(1, pc.transform.position + Vector3.ClampMagnitude((pc.transform.position - localDragPosition), 10f));

        // Blob rotation
        float angle = Vector3.Angle(Vector3.up, (pc.dragStartPos - currentDragPosition).normalized);
        angle = currentDragPosition.x < pc.dragStartPos.x ? 360f - angle : angle;
        currentBlob.transform.rotation = Quaternion.Euler(0, 0, angle);

        //Blob stretch
        float dist = Vector3.Distance(pc.dragStartPos, currentDragPosition);
        currentBlob.GetComponent<Blob>().Stretch(dist);

        currentScaleMultiplier = 1f + (Mathf.Clamp01(dist));
        currentBlob.GetComponent<Blob>().blobMesh.transform.localScale *= currentScaleMultiplier;
    }

    public override void OnDragEnd(GameObject currentBlob, bool mouse = false)
    {
        if (!pc.dragStarted) return;

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        dragReleasePos.z = 0f;

        line.positionCount = 0;

        if (Vector3.Distance(pc.dragStartPos, dragReleasePos) < 0.5f)
        {
            GameObject.Destroy(currentBlob);
            pc.InstantiateNewBlob();
            pc.dragStarted = false;
            return;
        }

        Vector3 shootDirection = (pc.dragStartPos - dragReleasePos).normalized;
        currentBlob.GetComponent<Blob>().Shoot(shootDirection, pc.force);
        currentBlob.GetComponent<Blob>().blobMesh.transform.localScale = Vector3.one * currentScaleMultiplier;

        pc.StartCooldown();
        pc.dragStarted = false;
    }
}
