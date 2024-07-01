using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindItem : MonoBehaviour
{
   public void FindNextCube()
    {
        BoosterManager.Ins.FindNextCubeByColor(LevelManager.Ins.currentColor);
    }
}
