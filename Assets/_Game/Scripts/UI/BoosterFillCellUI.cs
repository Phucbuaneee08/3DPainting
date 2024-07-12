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
    public Image imgBG;
    public RectTransform rectTransform;
    public Transform tf;
    public Vector2 initialPosition;
    public bool isUseBooster = false;
    //BoosterController boosterController;
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
        //boosterController = GetComponentInParent<BoosterController>();
        
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
      
        if (DataManager.Ins.playerData.boosterQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(id);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else
        {
            if (isUseBooster == false)
            {
                BoosterManager.Ins.IsCanUseZoomBooster = true;
                BoosterManager.Ins.ZoomBoosterByColor();

                BoosterManager.Ins.IsCanUseFillAllNumberBooster = true;
                BoosterManager.Ins.IsCanUseFillByNumberBooster = false;

                //boosterController.SetUpDown(BoosterManager.Ins.IDSelectBooster);
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
                BoosterManager.Ins.IsCanUseZoomBooster = false;
                //boosterController.SetUpDown(BoosterManager.Ins.IDSelectBooster);
                isUseBooster = false;
            }
        }
    }
    public void Btn_BoosterFillByNumber()
    {
        if (DataManager.Ins.playerData.boosterFillByColorQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(id);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else
        {
            if (isUseBooster == false)
            {
                BoosterManager.Ins.IsCanUseFillByNumberBooster = true;
                BoosterManager.Ins.IsCanUseZoomBooster = false;
                //boosterController.SetUpDown(BoosterManager.Ins.IDSelectBooster);
                if (LevelManager.Ins.currentColor != 0)
                {
                    //UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).SetMovePosition();
                    ItemManager.Ins.TurnOffAllColorItem();
                }

                LevelManager.Ins.ReleaseFocusCube();
                isUseBooster = true;
                return;
            }
            if (isUseBooster == true)
            {
                BoosterManager.Ins.IsCanUseFillByNumberBooster = false;
                //boosterController.SetUpDown(BoosterManager.Ins.IDSelectBooster);
                if (LevelManager.Ins.currentColor != 0)
                {
                    UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(LevelManager.Ins.currentColor).MoveUp();
                }
                isUseBooster = false;
            }
        }
    }

}
