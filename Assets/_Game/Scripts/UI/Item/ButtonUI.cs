using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public List<ButtonCellUI> listCell = new List<ButtonCellUI>();
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    public void SetUIBtn(int idSlect)
    {
        for (int i = 0; i < listCell.Count; i++)
        {
            if (listCell[i].isActive != false)
            {
                if (listCell[i].idSelect == idSlect)
                {
                    /*if (listCell[i].CurrentItemState == ItemState.TurnOff)// set up/down btn
                    {
                        listCell[i].ChangeItemState(ItemState.TurnOn);
                    }*/
                }
                else
                {
                    //listCell[i].ChangeItemState(ItemState.TurnOff);//set up/down btn
                }
            }
        }
    }
    public void SetUIStart(int i)
    {
        listCell[i].SetImgBg(sprites[0]);
    }
    public void SetSpritesBtn( int i, int idSlect)
    {
        if (listCell[i].isActive != false)
        {
            if (listCell[i].idSelect == idSlect)
            {
                listCell[i].SetImgBg(sprites[0]);
            }
            else
            {
                listCell[i].SetImgBg(sprites[1]);
            }
        }

    }
    public void SetCurrenState(int index)
    {
        listCell[index].CurrentItemState = ItemState.TurnOn;
    }
    public void LoadUIButtonItem()
    {
        SetCellActive(0, true);
        SetCellActive(2, true);
        SetCellActive(4, true);
        if (DataManager.Ins.playerData.CountLevelPassed >= 3)
        {
            if (DataManager.Ins.playerData.isShowDailyRewardFirst == true)
            {
                SetCellActive(1, true);
            }
            else
            {
                SetCellActive(1, false);
            }
        }
        if (DataManager.Ins.playerData.CountLevelPassed >= 5)
        {
            if (DataManager.Ins.playerData.isShowSpinRewardFirst == true)
            {
                SetCellActive(3, true);
            }
            else
            {
                SetCellActive(3, false);
            }
        }

        int activeCount = CountActiveCells();

        Vector2 cellSize = CalculateCellSize(activeCount);

        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.cellSize = cellSize;
        }
    }

    public void SetCellActive(int index, bool isActive)
    {
        if (index >= 0 && index < listCell.Count)
        {
            listCell[index].isActive = isActive;

        }
    }

    private int CountActiveCells()
    {
        int activeCount = 0;
        int id = 0;
        foreach (var cell in listCell)
        {
            if (cell.isActive)
            {
                activeCount++;
            }
            cell.SetData(id);
            id++;
        }
        return activeCount;
    }

    private Vector2 CalculateCellSize(int activeCount)
    {
        if (activeCount < 4)
        {
            return new Vector2(190, 190);
        }
        else
        {
            return new Vector2(180, 180);
        }
    }
    public void SetActiveCell(int index, bool isActive)
    {
        listCell[index].gameObject.SetActive(isActive);
        listCell[index].isActive = true;
        listCell[index].SetInitialPosition();
        int activeCount = CountIsActve();
        Vector2 cellSize = CalculateCellSize(activeCount);
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.cellSize = cellSize;
        }
    }
    private int CountIsActve()
    {
        int activeCount = 0;
        foreach (var cell in listCell)
        {
            if (cell.isActive)
            {
                activeCount++;
            }
        }
        return activeCount;
    }
}
