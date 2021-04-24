﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableComponent
{
    protected PlayerController pc;

    protected LineRenderer line;
    protected Vector3 dragStartPos;
    protected bool dragStarted;

    public DraggableComponent(PlayerController _pc)
    {
        pc = _pc;
        line = pc.GetComponent<LineRenderer>();
    }

    public virtual void OnDragStart(GameObject currentBlob, bool mouse = false)
    {
        if (pc.cOnCooldown != null) return;
        dragStarted = true;

        dragStartPos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        dragStartPos.z = 0f;

        line.positionCount = 2;
        line.SetPosition(0, pc.transform.position);
        line.SetPosition(1, pc.transform.position);
    }

    public virtual void OnDragMoved(GameObject currentBlob, bool mouse = false)
    {
        if (!dragStarted) return;

        //Move blob according to current touch position
        Vector3 currentDragPosition = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        currentDragPosition.z = 0;

        Vector3 localDragPosition = currentDragPosition + (pc.transform.position - dragStartPos);
        line.SetPosition(1, pc.transform.position + Vector3.ClampMagnitude((pc.transform.position - localDragPosition), 10f));

        // Blob rotation
        float angle = Vector3.Angle(Vector3.up, (dragStartPos - currentDragPosition).normalized);
        angle = currentDragPosition.x < dragStartPos.x ? 360f - angle : angle;
        currentBlob.transform.rotation = Quaternion.Euler(0, 0, angle);

        //Blob stretch
        float dist = Vector3.Distance(dragStartPos, currentDragPosition);
        currentBlob.GetComponent<Blob>().Stretch(dist);
    }

    public virtual void OnDragEnd(GameObject currentBlob, bool mouse = false)
    {
        if (!dragStarted) return;

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)pc.currentTouch.position);
        dragReleasePos.z = 0f;

        line.positionCount = 0;

        if (Vector3.Distance(dragStartPos, dragReleasePos) < 0.5f)
        {
            GameObject.Destroy(currentBlob);
            pc.InstantiateNewBlob();
            dragStarted = false;
            return;
        }

        Vector3 shootDirection = (dragStartPos - dragReleasePos).normalized;
        currentBlob.GetComponent<Blob>().Shoot(shootDirection, pc.force);

        pc.StartCooldown();
        dragStarted = false;
    }

    public virtual void ActivePower()
    {

    }

    public virtual void ResetPower()
    {

    }
}