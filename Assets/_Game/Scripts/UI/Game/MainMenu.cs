using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] TextMeshProUGUI textDiamond;

    private void Awake()
    {
        DataManager.Ins.playerData.OnGoldChanged += UpdateGoldText;
        DataManager.Ins.playerData.OnDiamondChanged += UpdateDiamondText;
    }

    public void Start()
    {
        UpdateGoldText(DataManager.Ins.playerData.Gold);
        UpdateDiamondText(DataManager.Ins.playerData.Diamond);
    }

    void UpdateGoldText(int gold)
    {
        textGold.text = gold.ToString();
    }

    void UpdateDiamondText(int diamond)
    {
        textDiamond.text = diamond.ToString();
    }

    public override void Setup()
    {
        base.Setup();
    }

    public override void Open()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        ReLoadData();
        base.Open();
    }

    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }
}
