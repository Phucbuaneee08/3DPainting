using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private List<ButtonCellUI> listCell = new List<ButtonCellUI>();
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    public void SetRecTF(int idSlect)
    {
        for (int i = 0; i < listCell.Count; i++)
        {
            if (listCell[i].isActive != false)
            {
                if (listCell[i].idSelect == idSlect)
                {
                    listCell[i].MoveUp();
                }
                else
                {
                    listCell[i].MoveDown();
                }
            }

        }
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
                Debug.Log("0");
            }
            else
            {
                Debug.Log("0 00");
                SetCellActive(1, false);
            }
               
        }
        if (DataManager.Ins.playerData.CountLevelPassed >= 5)
        {
            SetCellActive(3, true);
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
            return new Vector2(260, 160);
        }
        else if (activeCount == 4)
        {
            return new Vector2(210, 140);
        }
        else
        {
            return new Vector2(170, 120);
        }
    }
}
