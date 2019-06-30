using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputClick : MonoBehaviour
    ,IPointerClickHandler
{
    public NextAction input;

    public NextAction.Action action;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("1:" + input.action);
        input.action = action;
        //Debug.Log("2:" + input.action);
    }
}
