using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIGameplay : UICanvas
{
    [SerializeField] Transform content;
    [SerializeField] ColorItem colorItemPrefab;
    [SerializeField] List<ColorItem> colorItems;
    [SerializeField] private Timer timer;
    [SerializeField] RectTransform scrollViewRect;
    public Canvas canvas;
    public FillBoosterItem fillBoosterItem;
    public FillBoosterItem fillBoosterItem2;
    MiniPool<ColorItem> miniPool = new MiniPool<ColorItem>();

    public BoosterController boosterController;
    private void Awake()
    {
        miniPool.OnInit(colorItemPrefab, 10, content);
    }
    public override void Setup()
    {
        base.Setup();
        if (fillBoosterItem.IsState(FillBoosterState.TurnOn))
        {
            fillBoosterItem.ChangeBoosterItemState();
        }  
        if (fillBoosterItem2.IsState2(FillBoosterState.TurnOn))
        {
            fillBoosterItem2.ChangeBoosterFillItemState();
        }
        boosterController.LoadData();
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
        foreach (MaterialData md in listMD)
        {
            ColorItem colorItem = miniPool.Spawn();
            colorItem.SetData(md.colorID, md.material.color);
            colorItems.Add(colorItem);
        }
    }
    public ColorItem FindItemByColorId(int colorID)
    {
       
        foreach (ColorItem ci in colorItems)
        {
            if (ci.GetColorID() == colorID)
                return ci;
        }
        return null;
    }
    public void RemoveColorItem(int colorID)
    {
        foreach (ColorItem ci in colorItems)
        {
            if (ci.GetColorID() == colorID)
            {
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
    public bool CheckInput()
    {
  
        EventSystem eventSystem = EventSystem.current;
      
        List<RaycastResult> results = new List<RaycastResult>();
        // Tạo một PointerEventData mới, lấy thông tin từ input hiện tại
        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };
        // Lấy GraphicRaycaster từ canvas
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        // Thực hiện raycast và lưu trữ các kết quả vào danh sách
        raycaster.Raycast(eventData, results);
        // Nếu danh sách kết quả có bất kỳ phần tử nào, input đang chạm vào UI
        return results.Count > 0;
    }
    public bool CheckInputOnUI()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(scrollViewRect, Input.mousePosition, null);
    }
    public void SetZoomStateButton()
    {
        CameraManager.Ins.ChangeZoomState();
    }
    public void MoveDownBtn(FillBoosterItem fillBoosterItem)
    {
        fillBoosterItem.MoveDown();
    }
    public void MoveUpBtn(FillBoosterItem fillBoosterItem)
    {
        fillBoosterItem.MoveUp();
    }
}
