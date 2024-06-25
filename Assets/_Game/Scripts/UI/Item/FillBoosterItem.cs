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
                FillBooster.Ins.ChangeBoosterState(false);
                _state = FillBoosterState.TurnOff;
                transform.DOMoveY(transform.position.y - moveUpDistance, duration);

                break;
            case FillBoosterState.TurnOff:
                FillBooster.Ins.ChangeBoosterState(true);
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
