using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIGameplay : UICanvas
{
    [SerializeField] Transform content;
    [SerializeField] ColorItem colorItemPrefab;
    [SerializeField] List<ColorItem> colorItems;
    [SerializeField] private Timer timer;
    [SerializeField] RectTransform scrollViewRect;
    [SerializeField] FillBoosterItem fillBoosterItem;
    MiniPool<ColorItem> miniPool = new MiniPool<ColorItem>();
    private ColorItem colorItemSelected;


    private void Awake()
    {
        miniPool.OnInit(colorItemPrefab, 10, content);

    }
    public override void Setup()
    {
        base.Setup();
        if(fillBoosterItem.IsState(FillBoosterState.TurnOn))
        {
            fillBoosterItem.ChangeBoosterItemState();
        }
    }
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.GamePlay);
        
    }
    public void OpenSetting()
    {
        UIManager.Ins.OpenUI<UISetting>();
    }
    public void InitColorItem(List<MaterialData> listMD)
        {
            foreach(MaterialData md in  listMD)
            {
                ColorItem colorItem = miniPool.Spawn();
                colorItem.SetData(md.colorID,md.material.color);
                colorItems.Add(colorItem);
            }
        }
    public ColorItem FindItemByColorId(int colorID)
    {
        foreach(ColorItem ci in colorItems)
        {
            if (ci.GetColorID() == colorID)
                return ci;
        }
        return null;
    }
    public void RemoveColorItem(int colorID)
    {
        foreach(ColorItem ci in colorItems)
        {
            if (ci.GetColorID() == colorID){
                miniPool.Despawn(ci);
            }
        }
    }
    public void ResetColorItem()
    {
        foreach (ColorItem ci in colorItems)
        {
             miniPool.Despawn(ci);  
        }
        colorItems.Clear();
    }
    public void SetCountDownTime(int time)
    {
        timer.SetRemainTime(time);
    }
    public bool CheckInputOnUI()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(scrollViewRect, Input.mousePosition, null);
    }
  
    
}
