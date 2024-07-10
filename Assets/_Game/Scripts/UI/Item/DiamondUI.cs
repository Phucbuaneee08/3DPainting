using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDiamond;

    private int currentDiamond;

    void Start()
    {
        currentDiamond = DataManager.Ins.playerData.diamond;
        DataManager.Ins.OnDiamondChanged += UpdateDiamondText;
        textDiamond.text = currentDiamond.ToString();
    }

    public void UpdateDiamondText(int targetDiamond)
    {
        DOTween.To(() => currentDiamond, x => currentDiamond = x, targetDiamond, 0.3f)
            .OnUpdate(() =>
            {
                textDiamond.text = currentDiamond.ToString();
                currentDiamond = DataManager.Ins.playerData.diamond;
            });
    }

}
