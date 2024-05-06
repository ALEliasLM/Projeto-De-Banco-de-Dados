using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEnterEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MonoBehaviour[] OnHoverActivate;
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var m in OnHoverActivate) m.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var m in OnHoverActivate) m.enabled = false;
    }
}
