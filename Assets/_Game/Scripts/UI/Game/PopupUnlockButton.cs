using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class PopupUnlockButton : UICanvas
{
    [SerializeField] ObjButtonUnlock objBtnUnlockPrefab;
    [SerializeField] RectTransform tfEnd;

    protected RectTransform tfStart;
    private CanvasGroup canvasGroup;
    private ObjButtonUnlock objBtnUnlockInstance;

    public virtual void Awake()
    {
        canvasGroup = objBtnUnlockPrefab.GetComponent<CanvasGroup>();
    }
    public void UnlockButton(int index)
    {
        StartCoroutine(UnlockButtonRoutine(index));
    }
    private IEnumerator UnlockButtonRoutine(int index)
    {
        objBtnUnlockInstance = Instantiate(objBtnUnlockPrefab);
        objBtnUnlockInstance.transform.SetParent(tfEnd, false);
        objBtnUnlockInstance.transform.position = tfEnd.position;
        canvasGroup = objBtnUnlockInstance.GetComponent<CanvasGroup>() ?? objBtnUnlockInstance.gameObject.AddComponent<CanvasGroup>();
        yield return new WaitForSeconds(2f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(objBtnUnlockInstance.transform.DOMove(tfStart.position, 1.5f).SetEase(Ease.InOutQuad))
                .Join(objBtnUnlockInstance.transform.DOScale(Vector3.one / 2, 1.5f).SetEase(Ease.InOutQuad))
                .Join(canvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad))
                .OnComplete(() =>
                {
                    UIManager.Ins.GetUI<MainMenu>().SetActiveButton(index, true);
                });
        yield return sequence.WaitForCompletion();
        Destroy(objBtnUnlockInstance.gameObject);
    }

}
