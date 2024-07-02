using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] TextMeshProUGUI textDiamoind;
    private void Awake()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }
    public void Start()
    {

    }
    void Update()
    {
        textGold.text = DataManager.Ins.playerData.gold.ToString();
        textDiamoind.text = DataManager.Ins.playerData.diamond.ToString();
    }
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        ReLoadData();
        base.Open();
    }
    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }
}
