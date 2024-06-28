using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ColorItemState { Default = 0, IsSelected =1 }

public class ColorItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private int colorID;
    [SerializeField] private Text text;
    [SerializeField] private RectTransform element;
    [SerializeField] private ColorItemState colorItemState;

    public Vector2 initialPosition;

    private void Awake()
    {
        element = GetComponent<RectTransform>();
        StartCoroutine(IE_SetTFdata());
    }
    public void FocusCubeByColorID()
    {
        if (LevelManager.Ins.currentColor == colorID) return;
        BoosterManager.Ins.ZoomBoosterByColor();
        SetMovePosition();
        LevelManager.Ins.FocusByColorID(colorID);
    }
    public void SetData(int colorID,Color color) 
    { 
        this.colorID = colorID;
        bg.color = color;
        text.text = colorID.ToString();
        colorItemState = ColorItemState.Default;
        StartCoroutine(IE_SetTFdata());
    }
    IEnumerator IE_SetTFdata()
    {
        yield return new WaitForEndOfFrame();
        initialPosition = element.anchoredPosition;
    }
    public int GetColorID()
    {
        return colorID;
    }
    public void SetMovePosition()
    {
        switch (colorItemState)
        {
            case ColorItemState.Default:
                if (UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.IsState(FillBoosterState.TurnOn))
                {
                    UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.ChangeBoosterItemState();
                }
                if (UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.IsState2(FillBoosterState.TurnOn))
                {
                    UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.ChangeBoosterFillItemState();
                }
                colorItemState = ColorItemState.IsSelected;
                MoveUp();
                break;
            case ColorItemState.IsSelected:
                colorItemState= ColorItemState.Default;
                MoveDown();
                Debug.Log(element.position.y);
                break;
        }
    }
    public void SetFillAmount(float amount)
    {

        bg.fillAmount = amount;
    }
    public void MoveUp()
    {
        element.DOAnchorPosY(initialPosition.y + 40f, 0.3f);
    }

    public void MoveDown()
    {
        element.DOAnchorPosY(initialPosition.y, 0.5f);
    }
}
