using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBuyBooster : UICanvas
{
    public TextMeshProUGUI textCostBooster;
    public TextMeshProUGUI numberBooster;
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    public void SetData()
    {
        textCostBooster.text = BoosterManager.Ins.costBooster.ToString();
    }
}
