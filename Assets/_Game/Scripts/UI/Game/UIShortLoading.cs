using System;
using System.Collections;
using UnityEngine;

public class UIShortLoading : UICanvas
{
    private Action _onCloseAction;

    public UIShortLoading With(Action onCloseAction = null)
    {
        _onCloseAction = onCloseAction;
        StartCoroutine(CloseAfterDelay());
        return this;
    }
    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        _onCloseAction?.Invoke();
        UIManager.Ins.CloseUI<UIShortLoading>();
    }
}
