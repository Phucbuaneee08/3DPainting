using Paint3D;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class PopupClaim : UICanvas
{
    [SerializeField] private GameObject type1Obj;
    [SerializeField] private GameObject type2Obj;
    [SerializeField] private RewardTypeUI rewardTypeUISpin;
    private bool isClaim = false;
    public override void Open()
    { 
        base.Open();
       
        isClaim = false;
    }
    public void OnintSpin(SpinReward reward)
    {
        AudioManager.Ins.OnRewardSpin();
        type1Obj.SetActive(true);
        rewardTypeUISpin.InitSpin(reward);
    }
    public void BtnClaim()
    {
        if (isClaim) return;
        isClaim = true;
        rewardTypeUISpin.OnCollectSpin(1, () =>
        {
            UIManager.Ins.CloseUI<PopupClaim>();
            UIManager.Ins.GetUI<MainMenu>().uiSpin.isRotate = false;
            UIManager.Ins.GetUI<MainMenu>().uiSpin.UpdateBtn();
            UIManager.Ins.GetUI<MainMenu>().UIBtnSpin();
        });
    }
}
