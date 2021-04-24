using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDraggable : DraggableComponent
{
    private LineRenderer line2;

    public BlueDraggable(PlayerController _pc) : base(_pc)
    {
        PowerColorAttributes pca = _pc.DictPowerAttributes[PlayerController.PowerColor.BLUE];
        line.startColor = pca.lineRendererStartColor;
        line.endColor = pca.lineRendererEndColor;

        line2 = GameObject.Instantiate(new GameObject("line2"), pc.transform).AddComponent<LineRenderer>();
        line2.widthCurve = line.widthCurve;
        line2.numCapVertices = line.numCapVertices;
        line2.material = line.material;
        line2.startColor = pca.lineRendererStartColor;
        line2.endColor = pca.lineRendererEndColor;
        line2.positionCount = 0;
    }

    public override void OnDragStart(GameObject currentBlob, bool mouse = false)
    {
        if (pc.cOnCooldown != null) return;
        pc.dragStarted = true;

        pc.dragStartPos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        pc.dragStartPos.z = 0f;

        line.positionCount = 2;
        line.SetPosition(0, pc.transform.position);
        line.SetPosition(1, pc.transform.position);

        line2.positionCount = 2;
        line2.SetPosition(0, pc.transform.position);
        line2.SetPosition(1, pc.transform.position);
    }

    public override void OnDragMoved(GameObject currentBlob, bool mouse = false)
    {
        if (!pc.dragStarted) return;

        //Move blob according to current touch position
        Vector3 currentDragPosition = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        currentDragPosition.z = 0;

        Vector3 localDragPosition = currentDragPosition + (pc.transform.position - pc.dragStartPos);
        line.SetPosition(1, pc.transform.position + (Quaternion.Euler(0, 0, 10f) * Vector3.ClampMagnitude((pc.transform.position - localDragPosition), 10f)));

        line2.positionCount = 2;
        line2.SetPosition(0, pc.transform.position);
        line2.SetPosition(1, pc.transform.position + (Quaternion.Euler(0, 0, -10f) * Vector3.ClampMagnitude((pc.transform.position - localDragPosition), 10f)));

        // Blob rotation
        float angle = Vector3.Angle(Vector3.up, (pc.dragStartPos - currentDragPosition).normalized);
        angle = currentDragPosition.x < pc.dragStartPos.x ? 360f - angle : angle;
        currentBlob.transform.rotation = Quaternion.Euler(0, 0, angle);

        //Blob stretch
        float dist = Vector3.Distance(pc.dragStartPos, currentDragPosition);
        currentBlob.GetComponent<Blob>().Stretch(dist);
    }

    public override void OnDragEnd(GameObject currentBlob, bool mouse = false)
    {
        if (!pc.dragStarted) return;

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        dragReleasePos.z = 0f;

        line.positionCount = 0;
        line2.positionCount = 0;

        if (Vector3.Distance(pc.dragStartPos, dragReleasePos) < 0.5f)
        {
            GameObject.Destroy(currentBlob);
            pc.InstantiateNewBlob();
            pc.dragStarted = false;
            return;
        }

        Vector3 shootDirection = Quaternion.Euler(0, 0, 10f) * (pc.dragStartPos - dragReleasePos).normalized;
        currentBlob.GetComponent<Blob>().Shoot(shootDirection, pc.force);
        currentBlob.GetComponent<Blob>().blobMesh.transform.localScale = Vector3.one;

        Vector3 shootDirection2 = Quaternion.Euler(0, 0, -10f) * (pc.dragStartPos - dragReleasePos).normalized;
        GameObject twinBlob = GameObject.Instantiate(currentBlob.gameObject, currentBlob.transform.position, currentBlob.transform.rotation);
        twinBlob.GetComponent<Blob>().Shoot(shootDirection2, pc.force);

        pc.StartCooldown();
        pc.dragStarted = false;
    }

    public override void ResetPower()
    {
        GameObject.Destroy(line2.gameObject);
    }
}
