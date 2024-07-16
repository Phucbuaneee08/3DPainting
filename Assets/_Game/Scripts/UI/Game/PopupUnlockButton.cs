using System.Collections;
using UnityEngine;
using DG.Tweening;

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
    public override void Open()
    {
       
    }
    public void UnlockButton()
    {
        StartCoroutine(UnlockButtonRoutine());
    }
    private IEnumerator UnlockButtonRoutine()
    {
        objBtnUnlockInstance = Instantiate(objBtnUnlockPrefab);
        objBtnUnlockInstance.transform.SetParent(tfStart, false);
        objBtnUnlockInstance.transform.position = tfStart.position;
        canvasGroup = objBtnUnlockInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objBtnUnlockInstance.gameObject.AddComponent<CanvasGroup>();
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(objBtnUnlockInstance.transform.DOMove(tfEnd.position, 1.5f).SetEase(Ease.InOutQuad))
                .Join(objBtnUnlockInstance.transform.DOScale(Vector3.one * 2, 1.5f).SetEase(Ease.InOutQuad))
                .Join(canvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad));
        yield return sequence.WaitForCompletion();
        yield return new WaitForSeconds(0.8f);
        objBtnUnlockInstance.PlayAnim();
        yield return new WaitForSeconds(1.5f);
        sequence = DOTween.Sequence();
        sequence.Append(objBtnUnlockInstance.transform.DOMove(tfStart.position, 2f).SetEase(Ease.InOutQuad))
                .Join(objBtnUnlockInstance.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutQuad))
                .Join(canvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuad));
        yield return sequence.WaitForCompletion();
        objBtnUnlockInstance.gameObject.SetActive(false);
    }
}
