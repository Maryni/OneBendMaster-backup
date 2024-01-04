using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    #region private variables
    
    private UnityAction<int,int> actionOnEndDrag;
    private UnityAction actionOnEndDragWithoutParams;
    private UnityAction<int,int> actionOnDragWithParams;
    private UnityAction actionCheckConnection;
    private UnityAction<int,int> actionOnDragRemoveConnection;
    private int lastX = -1;
    private int lastY = -1;

    #endregion private variables

    #region public functions

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>())
            {
                Debug.Log($"[OnDrag] call, GO = {eventData.pointerCurrentRaycast.gameObject.name} | ElemType = {eventData.pointerCurrentRaycast.gameObject.GetComponent<BaseMatchThree>().ElementType}" );
                int x = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().X;
                int y = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().Y;
                actionOnDragRemoveConnection?.Invoke(x,y);
                if (x != lastX || y != lastY)
                {
                    actionOnEndDrag?.Invoke(x,y);
                    actionCheckConnection?.Invoke();
                    lastX = x;
                    lastY = y;
                    //Debug.Log($"[OnDrag] second point X = {lastX} | Y = {lastY}");
                    //Debug.Log($"[OnDrag] CheckConnection Invoked");
                }
                //Debug.Log($"[OnDrag] X ={x} | Y = {y}" );
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"lastX = {lastX} | lastY = {lastY}");
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>())
        {
            int x = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().X;
            int y = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().Y;
            if (lastX == -1 && lastY == -1)
            {
                actionOnDragWithParams(x,y);
                lastX = x;
                lastY = y;
                Debug.Log($"[OnBeginDrag] complete");
            }
            
            Debug.Log($"[OnBeginDrag] first point X = {lastX} | Y = {lastY}");
        }
        else
        {
            Debug.Log("wrong gameobject");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>())
            {
                int x = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().X;
                int y = eventData.pointerCurrentRaycast.gameObject.GetComponent<MatchThreeFlexibleElement>().Y;
                actionOnEndDrag?.Invoke(x,y);
                Debug.Log("OnEndDrag called");
                lastX = -1; //x
                lastY = -1;
            
            }
        }
        actionOnEndDragWithoutParams?.Invoke();
        Debug.Log($"[OnEndDrag] lastX ={lastX} | lastY = {lastY}" );
    }

    public void SetActionOnEndDrag(params UnityAction<int,int>[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnEndDrag += actions[i];
        }
    }
    public void SetActionOnEndDragWithoutParams(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnEndDragWithoutParams += actions[i];
        }
    }
    
    public void SetActionOnDragWithParams(params UnityAction<int, int>[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnDragWithParams += actions[i];
        }
    }

    public void SetActionCheckConnection(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionCheckConnection += actions[i];
        }
    }

    public void SetActionOnDragRemoveConnection(params UnityAction<int,int>[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnDragRemoveConnection += actions[i];
        }
    }
    
    #endregion public functions
}
