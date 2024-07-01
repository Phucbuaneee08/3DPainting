using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    public List<BoosterFillCellUI> listBooster = new List<BoosterFillCellUI>();
    public TextMeshProUGUI textNumber;
    public TextMeshProUGUI textNumber2;
    private void Start()
    {
        LoadataList();
    }
    public void Update()
    {
        //if (BoosterManager.Ins._isCanUseFillBooster == true)
        {
            textNumber.text = DataManager.Ins.playerData.boosterQuantity.ToString();
        }
     //   if (BoosterManager.Ins._isCanUseFillByNumberBooster == true)
        {
            textNumber2.text = DataManager.Ins.playerData.boosterFillByColorQuantity.ToString();
        }
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
        textNumber.text = DataManager.Ins.playerData.boosterQuantity.ToString();
        textNumber2.text = DataManager.Ins.playerData.boosterFillByColorQuantity.ToString();
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
        SetUpDown(-1);
        BoosterManager.Ins._isCanUseFillBooster = false;
        BoosterManager.Ins._isCanUseFillByNumberBooster = false;
    }
    public void ReLoadUIBooster()
    {
        BoosterManager.Ins.iDSelectBooster = 0;
        SetUpDown(-1);
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
