using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class NestedScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ScrollRect parentScrollRect;
    private ScrollRect childScrollRect;
    private bool isDraggingVertical = false;
    private bool isDraggingHorizontal = false;

    private void Awake()
    {
        childScrollRect = GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPointerOverUIElement(eventData, childScrollRect))
        {
            Vector2 delta = eventData.delta;
            isDraggingVertical = Mathf.Abs(delta.y) > Mathf.Abs(delta.x);
            isDraggingHorizontal = !isDraggingVertical;

            if (isDraggingVertical)
            {
                parentScrollRect.OnBeginDrag(eventData);
                childScrollRect.enabled = false;
            }
            else
            {
                childScrollRect.OnBeginDrag(eventData);
                parentScrollRect.enabled = false; 
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggingVertical)
        {
            parentScrollRect.OnEndDrag(eventData);
            parentScrollRect.enabled = true; 
        }
        else if (isDraggingHorizontal)
        {
            childScrollRect.OnEndDrag(eventData);
            childScrollRect.enabled = true; 
        }

        isDraggingVertical = false;
        isDraggingHorizontal = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggingVertical)
        {
            parentScrollRect.OnDrag(eventData);
        }
        else if (isDraggingHorizontal)
        {
            childScrollRect.OnDrag(eventData);
        }
    }

    private bool IsPointerOverUIElement(PointerEventData eventData, ScrollRect scrollRect)
    {
        RectTransform rectTransform = scrollRect.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera);
    }
}