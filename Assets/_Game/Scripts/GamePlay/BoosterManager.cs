﻿using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class BoosterManager : Singleton<BoosterManager>
{
    [SerializeField] private Player player;

    private float maxDistance = 0.01f;
    private int boosterQuantity = 999;
    private int boosterFillByColorQuantity = 1000;

    private bool _isCanUseFillBooster = false;
    private bool _isCanUseZoomBooster = true;
    private bool _isCanUseFillByNumberBooster = false;
    bool isCountQuantity = false;
    private float delayFillBooster = 0.001f;



    private Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
    /*
    * Script cho booster zoom in và zoom out  ****************************************************************************************************
   */





    /*
    * Script cho booster zoom khi chọn Màu đầu tiên ****************************************************************************************************
   */
    public void ChangeBoosterState(bool state)
    {
        _isCanUseFillBooster = state;
#if UNITY_EDITOR
        Debug.Log("Turn Fill Booster :" + state);
#endif
    }
    public void ZoomBoosterByColor()
    {
        if (_isCanUseZoomBooster)
        {
            CameraManager.Ins.cam.DOFieldOfView((CameraManager.Ins.minZoom + CameraManager.Ins.checkPointZoom) / 2, 0.5f);
            _isCanUseZoomBooster = false;
        }
    }
    public void ResetZoomBooster()
    {
        _isCanUseZoomBooster = true;
    }
    private IEnumerator OnFilled(List<Cube> visited)
    {
        Debug.LogError(visited.Count);
        foreach (Cube cubez in visited)
        {
            if (cubez.IsState(CubeState.Colored)) yield return null;
            yield return new WaitForSeconds(delayFillBooster);
            LevelManager.Ins.OnFilledCube(cubez);
        }
    }


    /*
     * Script cho booster Fill màu ****************************************************************************************************
     */
    #region Booster Fill By Number

    private IEnumerator OnFilledColor(List<Cube> visited)
    {
        foreach (Cube cubez in new List<Cube>(visited))
        {
            if (cubez.IsState(CubeState.Colored)) continue;
            yield return new WaitForSeconds(delayFillBooster);
            LevelManager.Ins.OnFilledCube(cubez);
        }
    }

    public bool CheckBooterFillByNumber()
    {
        return boosterFillByColorQuantity > 0 && _isCanUseFillByNumberBooster;
    }
    public void BoosterFillByNumber(Cube currentCube)
    {
        List<Cube> visited = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();
        queue.Enqueue(currentCube);
        visited.Add(currentCube);
        while (queue.Count > 0)
        {
            Cube cube = queue.Dequeue();
            foreach (Vector3 direction in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(cube.transform.position, direction, out hit, maxDistance))
                {
                    Cube adjacentCube = hit.collider.GetComponent<Cube>();
                    if (adjacentCube != null && adjacentCube.GetColorID() == LevelManager.Ins.currentColor && !adjacentCube.IsState(CubeState.Colored) && !visited.Contains(adjacentCube))
                    {
                        queue.Enqueue(adjacentCube);
                        visited.Add(adjacentCube);
                        StartCoroutine(OnFilledColor(new List<Cube>(visited)));
                        if(isCountQuantity == false)
                        {
                            boosterFillByColorQuantity -= 1;
                            isCountQuantity = true;
                        }
                    }
                }
            }
        }
        isCountQuantity = false;
    }

    public void ChangeBoosterFillState(bool state)
    {
        _isCanUseFillByNumberBooster = state;
    }

    #endregion
    public bool CheckBoosterQuantity()
    {
        return boosterQuantity > 0 && _isCanUseFillBooster;
    }

    public void FillBoosterByColor(Cube currentCube)
    {
        List<Cube> visited = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();
        queue.Enqueue(currentCube);
        visited.Add(currentCube);


        while (queue.Count > 0)
        {
            Cube cube = queue.Dequeue();

            foreach (Vector3 direction in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(cube.transform.position, direction, out hit, maxDistance))
                {
                    Cube adjacentCube = hit.collider.GetComponent<Cube>();
                    if (adjacentCube != null && adjacentCube.GetColorID() == currentCube.GetColorID() && !adjacentCube.IsState(CubeState.Colored) && !visited.Contains(adjacentCube))
                    {

                        queue.Enqueue(adjacentCube);
                        visited.Add(adjacentCube);
                    }
                }
            }
        }
        StartCoroutine(OnFilled(visited));
        boosterQuantity -= 1;



#if UNITY_EDITOR
        Debug.Log("Remove cube ID " + currentCube.GetColorID() + ": " + visited.Count);
#endif
    }
}
