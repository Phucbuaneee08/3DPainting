using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using JetBrains.Annotations;
using System.Linq;
public enum BoosterType
{
    None = 0,
    FillByColor = 1,
    FillAllColor = 2,
    FindByColor = 3
}
public class BoosterManager : Singleton<BoosterManager>
{
    public List<BoosterDetail> boosterDetails;
    [SerializeField] private Player player;
    [SerializeField] private int numberCubeFillByNumber = 10;
    [SerializeField] private int numberCubeFill = 10;

    private float maxDistance = 0.01f;

    public int boosterQuantity = 999;
    public int boosterFillByColorQuantity = 999;

    public bool IsCanUseFillAllNumberBooster { get; set; }
    public bool IsCanUseZoomBooster { get; set; }
    public bool IsCanUseFillByNumberBooster { get; set; }

    bool isCountQuantity = false;
    bool isCountQuantity2 = false;
    private float delayFillBooster = 0.001f;


    public BoosterType SelectedBoosterType;
    public int costBooster = 0;

    private Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

 
    public void OnInit()
    {
        PlayerData playerData = DataManager.Ins.playerData;
        UIManager.Ins.GetUI<UIGameplay>().OnInitBoosterItem(
            playerData.boosterFillByColorQuantity,
            playerData.boosterFillAllColorQuantity,
            playerData.boosterFindByColorQuantity);
        ResetAllBooster();
    }
    public void ChangeBoosterState(bool state)
    {
        IsCanUseFillAllNumberBooster = state;
#if UNITY_EDITOR
        Debug.Log("Turn Fill Booster :" + state);
#endif
    }
    public void ZoomBoosterByColor()
    {
        if (IsCanUseZoomBooster)
        {
            CameraManager.Ins.cam.DOOrthoSize((CameraManager.Ins.minZoom + CameraManager.Ins.checkPointZoom) / 2, 0.5f);
            CameraManager.Ins.LerpInCreaseOrthoSize(1f);
            UIManager.Ins.GetUI<UIGameplay>().ChangeZoomButtonState(CameraState.ZoomIn);
            IsCanUseZoomBooster = false;
        }
    }

    public void ResetAllBooster()
    {
        IsCanUseFillAllNumberBooster = false;
        IsCanUseFillByNumberBooster = false;
        IsCanUseZoomBooster = true;
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
        if (currentColorID == 0) return;
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

        int qty = DataManager.Ins.playerData.boosterFindByColorQuantity -= 1;
        UIManager.Ins.GetUI<UIGameplay>().findByColorItem.SetQuantityText(qty);

        if (DataManager.Ins.playerData.boosterFindByColorQuantity <= 0)
        {
            ItemManager.Ins.TurnOffBoosterItemByEnum(BoosterType.FindByColor);
            //UIManager.Ins.GetUI<UIGameplay>().boosterController.ReLoadUIBooster();
        }

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
        ItemManager.Ins.TurnOffAllBoosterItem();

     

    }
   

    #endregion


    #region Booster Fill By Number
    public bool CheckBooterFillByNumber()
    {
        return DataManager.Ins.playerData.boosterFillByColorQuantity > 0 && IsCanUseFillByNumberBooster;
    }
    public void BoosterFillByNumber(Cube currentCube)
    {
        List<Cube> visited = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();
        queue.Enqueue(currentCube);
        visited.Add(currentCube);
        LevelManager.Ins.OnFilledCube(currentCube);
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
        StartCoroutine(OnFilled(visited));
        if (!isCountQuantity)
        {
            int qty = DataManager.Ins.playerData.boosterFillByColorQuantity -= 1;
            UIManager.Ins.GetUI<UIGameplay>().fillByColorItem.SetQuantityText(qty);
            isCountQuantity = true;
        }
        isCountQuantity = false;
        if (DataManager.Ins.playerData.boosterFillByColorQuantity <= 0)
        {
            ItemManager.Ins.TurnOffBoosterItemByEnum(BoosterType.FillByColor);
            //UIManager.Ins.GetUI<UIGameplay>().boosterController.ReLoadUIBooster();
        }
    }
    public void ChangeBoosterFillState(bool state)
    {
        IsCanUseFillByNumberBooster = state;
    }
    #endregion
    /*
     * Script cho booster Fill màu ****************************************************************************************************
     */
    #region Booster Fill All Number
    public bool CheckBoosterQuantity()
    {
        return DataManager.Ins.playerData.boosterFillAllColorQuantity > 0 && IsCanUseFillAllNumberBooster;
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
            int qty = DataManager.Ins.playerData.boosterFillAllColorQuantity -= 1;
            UIManager.Ins.GetUI<UIGameplay>().fillAllColorItem.SetQuantityText(qty);
            isCountQuantity2 = true;
        }
        isCountQuantity2 = false;
        if (DataManager.Ins.playerData.boosterFillAllColorQuantity <= 0)
        {
            //UIManager.Ins.GetUI<UIGameplay>().boosterController.ReLoadUIBooster();
            ItemManager.Ins.TurnOffBoosterItemByEnum(BoosterType.FillAllColor);
        }
#if UNITY_EDITOR
        Debug.Log("Remove cube ID " + currentCube.GetColorID() + ": " + visited.Count);
#endif
    }
    #endregion

    public int GetBoosterPrice(BoosterType boosterType)
    {
       return boosterDetails.FirstOrDefault(b => b.boosterType == boosterType).GetBoosterPrice();
    }
}

[System.Serializable]
public class BoosterDetail
{
    public BoosterType boosterType;
    [SerializeField] private int boosterPrice;

    public int GetBoosterPrice()
    {
        return boosterPrice;
    }
    
}