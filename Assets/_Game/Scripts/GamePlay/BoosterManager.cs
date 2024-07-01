using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using JetBrains.Annotations;

public class BoosterManager : Singleton<BoosterManager>
{
    [SerializeField] private Player player;
    [SerializeField] private int numberCubeFillByNumber = 10;
    [SerializeField] private int numberCubeFill = 10;

    private float maxDistance = 0.01f;

    public int boosterQuantity = 999;
    public int boosterFillByColorQuantity = 999;

    public bool _isCanUseFillBooster = false;
    public bool _isCanUseFillByNumberBooster = false;

    private bool _isCanUseZoomBooster = true;
    bool isCountQuantity = false;
    bool isCountQuantity2 = false;
    private float delayFillBooster = 0.001f;

    public int iDSelectBooster = 0;

    private Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
    public void OnReset()
    {
        _isCanUseFillBooster = false;
        _isCanUseFillByNumberBooster = false;
        _isCanUseZoomBooster = true;
    }
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

        foreach (Cube cubez in visited)
        {
            if (cubez.IsState(CubeState.Colored)) yield return null;
            yield return new WaitForSeconds(delayFillBooster);
            LevelManager.Ins.OnFilledCube(cubez);
        }
    }
    #region Booster Find Next Cube By Color
    public void FindNextCubeByColor(int currentColorID)
    {
        Cube cub = Ultilities.CheckNextCubeInList(LevelManager.Ins.Cubes, currentColorID);
        
        Vector3 cameraPosition = Camera.main.transform.position;


        Vector3 cubeDirection = cub.transform.position - player.transform.position;
        Vector3 playerDirection = cameraPosition - player.transform.position;


        Vector3 horizontalRotateCube = new Vector3(0, cubeDirection.y, cubeDirection.z);
        Vector3 horizontalRotatePlayer = new Vector3(0, playerDirection.y, playerDirection.z);

        float angle = Vector3.Angle(horizontalRotateCube, horizontalRotatePlayer);
        if (cubeDirection.y > 0)
            RotateXAxis(-angle, 1f);
        else
            RotateXAxis(angle, 1f);

      
        StartCoroutine(RotateYAxis(cub)); 
         Debug.Log(angle);
        
    }
    private void  RotateXAxis(float angle,float duration)
    {
     
        player.transform.DORotate(new Vector3(angle, 0, 0), duration, RotateMode.WorldAxisAdd);
    }
    private IEnumerator RotateYAxis(Cube cub)
    {
        yield return new WaitForSeconds(1f);

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cubeDirection = cub.transform.position - player.transform.position;
        Vector3 playerDirection = cameraPosition - player.transform.position;

        Vector3 horizontalRotateCube = new Vector3(cubeDirection.x, 0, cubeDirection.z);
        Vector3 horizontalRotatePlayer = new Vector3(playerDirection.x, 0, playerDirection.z);

        float angle2 = Vector3.Angle(horizontalRotateCube, horizontalRotatePlayer);

        Debug.Log(player.transform.up.y );

        if(player.transform.up.y > 0)
        {
            if (cubeDirection.x > 0) player.transform.DORotate(new Vector3(0, angle2, 0), 1f, RotateMode.LocalAxisAdd);
            else player.transform.DORotate(new Vector3(0, -angle2, 0), 1f, RotateMode.LocalAxisAdd);
        }
        else
        {
            if (cubeDirection.x > 0) player.transform.DORotate(new Vector3(0, -angle2, 0), 1f, RotateMode.LocalAxisAdd);
            else player.transform.DORotate(new Vector3(0, +angle2, 0), 1f, RotateMode.LocalAxisAdd);
        }
       

     

    }


    #endregion


    #region Booster Fill By Number

    private IEnumerator OnFilledColor(List<Cube> visited)
    {
        foreach (Cube cubez in visited)
        {
            if (cubez.IsState(CubeState.Colored)) continue;
            yield return new WaitForSeconds(delayFillBooster);
            LevelManager.Ins.OnFilledCube(cubez);
        }
    }

    public bool CheckBooterFillByNumber()
    {
        return DataManager.Ins.playerData.boosterFillByColorQuantity > 0 && _isCanUseFillByNumberBooster;
    }
    public void BoosterFillByNumber(Cube currentCube)
    {
        List<Cube> visited = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();

        queue.Enqueue(currentCube);
        visited.Add(currentCube);
        int totalProcessed = 0;
        while (queue.Count > 0 && totalProcessed < numberCubeFillByNumber)
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
                        totalProcessed++;
                        if (totalProcessed >= numberCubeFillByNumber) break;
                    }
                }
            }
        }
        StartCoroutine(OnFilledColor(visited));
        if (!isCountQuantity)
        {
            DataManager.Ins.playerData.boosterFillByColorQuantity -= 1;
            isCountQuantity = true;
        }
        isCountQuantity = false;
    }
    public void ChangeBoosterFillState(bool state)
    {
        _isCanUseFillByNumberBooster = state;
    }
    #endregion
    /*
     * Script cho booster Fill màu ****************************************************************************************************
     */
    #region Booster Fill All Number
    public bool CheckBoosterQuantity()
    {
        return DataManager.Ins.playerData.boosterQuantity > 0 && _isCanUseFillBooster;
    }

    public void FillBoosterByColor(Cube currentCube)
    {
        List<Cube> visited = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();
        queue.Enqueue(currentCube);
        visited.Add(currentCube);
        LevelManager.Ins.OnFilledCube(currentCube);
        // Set color for first cube 
        //MaterialManager.Ins.SetColor(currentCube, currentCube.GetColorID());
        //currentCube.ChangeState(CubeState.Colored);


        int totalProcessed = 0;
        while (queue.Count > 0 && totalProcessed < numberCubeFill)
        {
            Cube cube = queue.Dequeue();

            foreach (Vector3 direction in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(cube.transform.position, direction, out hit, maxDistance))
                {
                    Cube adjacentCube = hit.collider.GetComponent<Cube>();
                    if (adjacentCube != null /*&& adjacentCube.GetColorID() == currentCube.GetColorID()*/ && !adjacentCube.IsState(CubeState.Colored) && !visited.Contains(adjacentCube))
                    {
                        queue.Enqueue(adjacentCube);
                        visited.Add(adjacentCube);
                        totalProcessed++;
                        if (totalProcessed >= numberCubeFill) break;
                    }
                }
            }
        }
        StartCoroutine(OnFilled(visited));
        if (!isCountQuantity2)
        {
            DataManager.Ins.playerData.boosterQuantity -= 1;
            isCountQuantity2 = true;
        }
        isCountQuantity2 = false;
#if UNITY_EDITOR
        Debug.Log("Remove cube ID " + currentCube.GetColorID() + ": " + visited.Count);
#endif
    }
    #endregion
}
