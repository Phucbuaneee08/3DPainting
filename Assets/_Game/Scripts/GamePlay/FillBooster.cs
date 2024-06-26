using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FillBooster : Singleton<FillBooster>
{
    [SerializeField] private Player player;
    private float maxDistance = 0.01f;
    private int boosterQuantity = 999;
    private bool _isCanUseFillBooster = false;
    private bool _isCanUseZoomBooster = true;
    private Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
  
    public bool CheckBoosterQuantity()
    {
        return boosterQuantity > 0 && _isCanUseFillBooster;
    }
    public void ChangeBoosterState(bool state)
    {
        _isCanUseFillBooster = state;
#if UNITY_EDITOR
        Debug.Log("Turn Fill Booster :"+ state);
#endif
    }
    public void ZoomBoosterByColor()
    {
        if (_isCanUseZoomBooster)
        {
            CameraManager.Ins.cam.DOFieldOfView((CameraManager.Ins.minZoom+CameraManager.Ins.checkPointZoom)/2,0.5f);
            _isCanUseZoomBooster = false;
        }
    }
    public void ResetZoomBooster()
    {
        _isCanUseZoomBooster = true;
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
                    if (adjacentCube != null && adjacentCube.GetColorID() == currentCube.GetColorID()&& !adjacentCube.IsState(CubeState.Colored) && !visited.Contains(adjacentCube))
                    {
                      
                        queue.Enqueue(adjacentCube);
                        visited.Add(adjacentCube);
                    }
                }
            }
        }

        foreach(Cube cubez in visited)
        {
            if (cubez.IsState(CubeState.Colored)) return;
            LevelManager.Ins.OnFilledCube(cubez);
            //MaterialManager.Ins.SetColor(cube, cube.GetColorID());
        }
        boosterQuantity -= 1;
#if UNITY_EDITOR
        Debug.Log("Remove cube ID "+currentCube.GetColorID()+": "+visited.Count);
#endif
    }
}
