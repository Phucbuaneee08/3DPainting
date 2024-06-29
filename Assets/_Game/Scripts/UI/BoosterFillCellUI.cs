using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterFillCellUI : MonoBehaviour
{
    public int id;
    public TextMeshProUGUI textNumber;
    public Image imgBG;
    public RectTransform rectTransform;
    public Transform tf;
    public Vector2 initialPosition;
    public bool isUseBooster = false;
    BoosterController boosterController;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(IE_SetTFdata());
    }
    IEnumerator IE_SetTFdata()
    {
        yield return new WaitForEndOfFrame();
        initialPosition = rectTransform.anchoredPosition;
    }
    public void Start()
    {
        boosterController = GetComponentInParent<BoosterController>();
    }
    public void MoveUp()
    {
        rectTransform.DOAnchorPosY(initialPosition.y + 40f, 0.5f);
    }

    public void MoveDown()
    {
        rectTransform.DOAnchorPosY(initialPosition.y, 0.5f);
    }
    public void SetGB(Image image)
    {
        imgBG = image;
    }
    public void GetID(int _id)
    {
        id = _id;
    }
    public void Btn_BoosterFillItem()
    {
        if (isUseBooster == false)
        {
            BoosterManager.Ins.iDSelectBooster = id;
            BoosterManager.Ins._isCanUseFillBooster = true;
            BoosterManager.Ins._isCanUseFillByNumberBooster = false;
            boosterController.SetUpDown(BoosterManager.Ins.iDSelectBooster);
            if (LevelManager.Ins.currentColor != 0)
            {
                UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).MoveDown();
            }
            LevelManager.Ins.ReleaseFocusCube();
            isUseBooster = true;
            return;
        }
        if (isUseBooster == true)
        {
            BoosterManager.Ins.iDSelectBooster = 0;
            BoosterManager.Ins._isCanUseFillBooster = false;
            boosterController.SetUpDown(BoosterManager.Ins.iDSelectBooster);
            isUseBooster = false;
        }

    }
    public void Btn_BoosterFillByNumber()
    {
        if (isUseBooster == false)
        {
            BoosterManager.Ins.iDSelectBooster = id;
            BoosterManager.Ins._isCanUseFillByNumberBooster = true;
            BoosterManager.Ins._isCanUseFillBooster = false;
            boosterController.SetUpDown(BoosterManager.Ins.iDSelectBooster);
            if(LevelManager.Ins.currentColor!=0)
            {
                UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).SetMovePosition();
            }
            
            LevelManager.Ins.ReleaseFocusCube();
            isUseBooster = true;
            return;
        }
        if (isUseBooster == true)
        {
            BoosterManager.Ins.iDSelectBooster = 0;
            BoosterManager.Ins._isCanUseFillByNumberBooster = false;
            boosterController.SetUpDown(BoosterManager.Ins.iDSelectBooster);
            if (LevelManager.Ins.currentColor != 0)
            {
                UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).MoveUp();
            }
            isUseBooster = false;
        }
    }

}
