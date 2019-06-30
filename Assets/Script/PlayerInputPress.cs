using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputPress : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
{
    public NextAction input;

    public NextAction.Action action;

    private bool pressed = false;

    void Update()
    {
        if(pressed && input.action != action)
        {
            input.action = action;
        }
        else if(!pressed && input.action == action)
        {
            input.action = NextAction.Action.none;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Press");
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Leave");
        pressed = false;
    }
}
