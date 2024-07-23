using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PopupRate : UICanvas
{
    public Button send;
    [SerializeField] private List<StarUI> listStars;
    public override void Open()
    {
        base.Open();
        Oninit();
    }
    public void Oninit()
    {
        for (int i = 0; i < listStars.Count; i++)
        {
            listStars[i].id = i;
        }
    }
    public void StarClicked(int starId)
    {
        for (int i = 0; i < listStars.Count; i++)
        {
            if (i <= starId)
            {
                listStars[i].ActiveObj(true);
            }
            else
            {
                listStars[i].ActiveObj(false);
            }
        }
    }
    public void Btn_Exit()
    {
        UIManager.Ins.CloseUI<PopupRate>();
    }
}
