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
    private FillBoosterState _state= FillBoosterState.TurnOff;
    private float moveUpDistance = 50f;
    private float duration = 0.5f;
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
                if (LevelManager.Ins.currentColor != 0) {
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
        switch (_state)
        {
            case FillBoosterState.TurnOn:
                BoosterManager.Ins.ChangeBoosterFillState(false);
                _state = FillBoosterState.TurnOff;
                transform.DOMoveY(transform.position.y - moveUpDistance, duration);

                break;
            case FillBoosterState.TurnOff:
                if (LevelManager.Ins.currentColor != 0)
                {
                   // UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).SetMovePosition();
                    LevelManager.Ins.ReleaseFocusCube();

                }
                BoosterManager.Ins.ChangeBoosterFillState(true);
                _state = FillBoosterState.TurnOn;
                transform.DOMoveY(transform.position.y + moveUpDistance, duration);

                break;
        }
    }
    public bool IsState(FillBoosterState state)
    {
        return _state == state;
    }
}
