using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGold;
    private int currentGold;
    private void Start()
    {
        currentGold = DataManager.Ins.playerData.gold;
        DataManager.Ins.OnGoldChanged += UpdateGoldText;
        textGold.text = currentGold.ToString();
    }
    public void UpdateGoldText(int targetGold)
    {
        DOTween.To(() => currentGold, x => currentGold = x, targetGold, 0.75f)
            .OnUpdate(() =>
            {
                textGold.text = currentGold.ToString();
                currentGold = DataManager.Ins.playerData.gold;
            });
    }
}
