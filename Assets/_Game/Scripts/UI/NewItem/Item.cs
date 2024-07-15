using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    [SerializeField] protected int _id;
    [SerializeField] protected RectTransform rectTransform; // v? trí c?a item 
    [SerializeField] protected Vector3 initialPosition; //v? trí kh?i t?o 
    [SerializeField] protected float moveDistance = 40f;
    public ItemState CurrentItemState { get;  set; }
    public int ID => _id;
    public virtual void OnInit() { }
    public virtual void TurnOn() { }
    public virtual void TurnOff() { }


    public virtual void OnClick() { }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(IE_SetInitPosition());
        CurrentItemState = ItemState.TurnOff;
    }

    public void ChangeItemState(ItemState itemState)
    {
        switch (itemState)
        {
            case ItemState.TurnOn:
                if (CurrentItemState == ItemState.TurnOff) 
                {
                    this.TurnOn();
                    CurrentItemState = ItemState.TurnOn;  
                }

                break;
            case ItemState.TurnOff: 
                if(CurrentItemState == ItemState.TurnOn)
                {
                    this.TurnOff();
                    CurrentItemState = ItemState.TurnOff;
                }
                break;
        }
    }

   
    IEnumerator IE_SetInitPosition()
    {
        yield return new WaitForEndOfFrame();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void MoveUp()
    {
        rectTransform.DOAnchorPosY(initialPosition.y + moveDistance, 0.5f);
    }
    public void MoveDown()
    {
        rectTransform.DOAnchorPosY(initialPosition.y, 0.5f);
    }

}
public enum ItemState
{
    TurnOn=0,
    TurnOff=1
}
