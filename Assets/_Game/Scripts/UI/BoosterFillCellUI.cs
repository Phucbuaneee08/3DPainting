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
    
   

}
