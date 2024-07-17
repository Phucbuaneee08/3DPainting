using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private List<ButtonCellUI> listCell = new List<ButtonCellUI>();
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    public void LoadUIButtonItem(PlayerData playerData)
    {
        SetCellActive(0, true);
        SetCellActive(2, true);
        SetCellActive(4, true);

        if (playerData.CountLevelPassed >= 3)
        {
            SetCellActive(1, true);
        }
        if (playerData.CountLevelPassed >= 5)
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

    private void SetCellActive(int index, bool isActive)
    {
        if (index >= 0 && index < listCell.Count)
        {
            listCell[index].isActive = isActive;
           
        }
    }

    private int CountActiveCells()
    {
        int activeCount = 0;
        foreach (var cell in listCell)
        {
            if (cell.isActive)
            {
                activeCount++;
            }
            cell.SetData();
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
