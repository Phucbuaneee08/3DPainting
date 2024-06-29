using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    public List<BoosterFillCellUI> listBooster = new List<BoosterFillCellUI>();

    private void Start()
    {
        LoadataList();
    }
    private void LoadataList()
    {
        if (listBooster.Count > 0)
        {
            for (int i = 0; i < listBooster.Count; i++)
            {
                listBooster[i].GetID(i + 1);
                listBooster[i].isUseBooster = false;
            }
        }
    }
    public void LoadData()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(IE_LoadData());
        }
    }

    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        BoosterManager.Ins.iDSelectBooster = 0;
        SetUpDown(0);
        BoosterManager.Ins._isCanUseFillBooster = false;
        BoosterManager.Ins._isCanUseFillByNumberBooster = false;
    }
    public void SetUpDown(int _id)
    {
        for (int i = 0; i < listBooster.Count; i++)
        {
            if (listBooster[i].id == _id)
            {
                listBooster[i].MoveUp();

            }
            else
            {
                listBooster[i].MoveDown();
            }
        }
       
    }
}
