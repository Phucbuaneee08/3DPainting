using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using DG.Tweening;
public enum FillBoosterState
{
    TurnOn = 0,
    TurnOff = 1,
}
public class FillBoosterItem : MonoBehaviour
{
    [SerializeField] private RectTransform element;

    private FillBoosterState _state = FillBoosterState.TurnOff;
    public FillBoosterState _state2 = FillBoosterState.TurnOff;
    private float moveUpDistance = 50f;
    private float duration = 0.5f;
    public Vector2 initialPosition;

    private void Awake()
    {
        element = GetComponent<RectTransform>();
    }
    public void ChangeBoosterItemState()
    {
        switch (_state)
        {
            case FillBoosterState.TurnOn:
                BoosterManager.Ins.ChangeBoosterState(false);
                _state = FillBoosterState.TurnOff;
                transform.DOMoveY(transform.position.y - moveUpDistance, duration);
                break;
            case FillBoosterState.TurnOff:
                if (LevelManager.Ins.currentColor != 0)
                {
                    UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).SetMovePosition();
                    LevelManager.Ins.ReleaseFocusCube();
                }
                BoosterManager.Ins.ChangeBoosterState(true);
                _state = FillBoosterState.TurnOn;
                transform.DOMoveY(transform.position.y + moveUpDistance, duration);
                break;
        }
    }
    public void ChangeBoosterFillItemState()
    {
        switch (_state2)
        {
            case FillBoosterState.TurnOn:
                BoosterManager.Ins.ChangeBoosterFillState(false);
                _state2 = FillBoosterState.TurnOff;
                UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem2.transform.DOMoveY(transform.position.y - moveUpDistance, duration);
                break;
            case FillBoosterState.TurnOff:
                if (LevelManager.Ins.currentColor != 0)
                {
                    UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).SetMovePosition();
                    LevelManager.Ins.ReleaseFocusCube();
                }
                BoosterManager.Ins.ChangeBoosterFillState(true);
                _state2 = FillBoosterState.TurnOn;
                UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem2.transform.DOMoveY(transform.position.y + moveUpDistance, duration);
                break;
        }
    }
    public void Btn_BoosterFillByID()
    {

    }
    public bool IsState(FillBoosterState state)
    {
        return _state == state;
    } 
    public bool IsState2(FillBoosterState state)
    {
        return _state2 == state;
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
