using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private List<Level> levels;
    [SerializeField] private List<Cube> cubes;
    [SerializeField] private List<CubeType> cubeTypes;
    [SerializeField] private  int totalTime;
    [SerializeField] private int reviveTime;
    private bool _isCanRevive;
    public Level currentLevel;
    public int currentColor;
    public int cubeTotal;

    public void OnInit()
    {
        currentColor = 0;
        _isCanRevive = true;
        player.gameObject.SetActive(true);
        cubeTotal = currentLevel.cubes.Count;
        if (currentLevel.materials.Count > 0)
        {
            foreach(MaterialData material in currentLevel.materials)
            {
                int count = 0;
                foreach(CubeData cube in currentLevel.cubes)
                {
                    if(cube.realColorID == material.colorID)
                    {
                        count++;
                    }
                }
                cubeTypes.Add(new CubeType(material.colorID, count,count));
            }
        }
    }

    public void FocusByColorID(int colorID)
    {

        if(currentColor == 0)
        {
            Focus(colorID);
            this.currentColor = colorID;
            return;
        }
        if (currentColor != colorID) 
        {
            UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(currentColor).SetMovePosition();
            Focus(colorID);
            this.currentColor = colorID;
        }
        
    }
    public void OnReset()
    {
        player.OnReset();
        cubeTypes.Clear();
        foreach (Cube cube in cubes)
        {
            SimplePool.Despawn(cube);
        }
        cubes.Clear();
        SimplePool.CollectAll();
    }

    public void OnLoadLevel(Level level)
    {

        currentLevel = level;
        OnInit();
        CameraManager.Ins.SetZoomInfo(currentLevel.zoomInfo);
        MaterialManager.Ins.SetMatData(currentLevel.materials);
        for (int i = 0; i < currentLevel.cubes.Count; i++)
        {
            Cube newCube = SimplePool.Spawn<Cube>(PoolType.Cube, currentLevel.cubes[i].position, Quaternion.identity);
            newCube.SetCubeData(i, currentLevel.cubes[i].realColorID, currentLevel.cubes[i].defaultColorID);
            MaterialManager.Ins.SetDefaultColor(newCube, newCube.GetColorID()-1);
            cubes.Add(newCube);
        }
        UIManager.Ins.OpenUI<UIGameplay>().InitColorItem(currentLevel.materials);
        UIManager.Ins.GetUI<UIGameplay>().SetCountDownTime(totalTime);
    }
   
    public void OnFilledCube(Cube cube)
    {
        if (cube.IsState(CubeState.Colored)) return;
        MaterialManager.Ins.SetColor(cube, cube.GetColorID());
        cube.ChangeState(CubeState.Colored);
        ////ParticlePool.Play(ParticleType.Hit_1, cube.TF);
        RemoveCubeByColorID(cube.GetColorID());
//#if UNITY_EDITOR
//        cube.gameObject.SetActive(false);
//#endif

        if (cubeTotal == 0)
        {
            //UIManager.Ins.OpenUI<UIVictory>();
            StartCoroutine(OnCelebration());
        }

    }
    public void RemoveCubeByColorID(int colorID)
    {
        foreach(CubeType cubes in cubeTypes) 
        { 
            if(cubes.colorID == colorID)
            {

                cubes.quantity--;
                cubeTotal--;
                StartCoroutine(OnRemoveCube(cubes));
                if (cubes.quantity == 0)
                {
                    if (cubeTypes.Count > 0 && cubeTotal>0 && !FillBooster.Ins.CheckBoosterQuantity()) {                 
                        CubeType currentColorType = Ultilities.CheckNextCubeTypeInList(cubeTypes,currentColor);
                        currentColor = currentColorType.colorID;
                        if(currentColorType.colorID!=0)
                            UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(currentColor).SetMovePosition();
                        Focus(currentColor); 
                    }
                    UIManager.Ins.GetUI<UIGameplay>().RemoveColorItem(colorID);
                }               
            }
        }
        
    }
    private IEnumerator OnRemoveCube(CubeType cubes)
    {
        yield return null;
      
        UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(cubes.colorID).SetFillAmount((float)cubes.quantity/(float)cubes.total);
    }
    private IEnumerator OnCelebration()
    {
        CameraManager.Ins.SetFieldOfView();
        player.MoveToStartPosition();
        player.PlayAnim();
        yield return new WaitForSeconds(2f);
        Victory();
    }
    public void Fail()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIFail>();
    }
    public void Victory()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIVictory>();
       
    }
    public void CheckReviveOrFail()
    {
        if(_isCanRevive)
        {
            _isCanRevive = false;
            UIManager.Ins.OpenUI<UIRevive>();
        }
        else
        {
            Fail();
        }
    }
    public void Revive()
    {
        UIManager.Ins.GetUI<UIGameplay>().SetCountDownTime(reviveTime);
    }
    public void Home()
    {
        UIManager.Ins.GetUI<UIGameplay>().ResetColorItem();
        CameraManager.Ins.Reset();
        FillBooster.Ins.ResetZoomBooster();
        UIManager.Ins.CloseAll();
        OnReset();
        UIManager.Ins.OpenUI<UIMainMenu>();
        
    }
    //public void OnLoadLevel(int level)
    //{

    //    currentLevel = levels[level];
    //    MaterialManager.Ins.SetMatData(currentLevel.materials);
    //    for (int i = 0; i < currentLevel.cubes.Count; i++)
    //    {
    //        Cube newCube = SimplePool.Spawn<Cube>(PoolType.Cube, currentLevel.cubes[i].position, Quaternion.identity);
    //        newCube.SetCubeData(i, currentLevel.cubes[i].realColorID, currentLevel.cubes[i].defaultColorID);
    //        MaterialManager.Ins.SetColor(newCube, newCube.GetDefaultColorID());
    //        cubes.Add(newCube);
    //    }

    //    UIManager.Ins.OpenUI<UIGameplay>().InitColorItem(currentLevel.materials);

    //}

    public void Zoomin()
    {
        foreach (Cube cube in cubes)
        {
            switch (cube.GetState())
            {
                case CubeState.Colored:
                    break;
                case CubeState.Focus:
                    MaterialManager.Ins.SetHighLightColor(cube);
                    break;
                default:
                    MaterialManager.Ins.SetShowTextColor(cube);
                    cube.ChangeState(CubeState.Zoomin);
                    break;
            }
        }
    }
    public void Zoomout()
    {
        foreach (Cube cube in cubes)
        {
            switch (cube.GetState())
            {
                case CubeState.Colored:
                    break;
                case CubeState.Focus:
                    MaterialManager.Ins.SetDefaultColor(cube, cube.GetColorID()-1);

                    break;
                default:
                    MaterialManager.Ins.SetDefaultColor(cube, cube.GetColorID() - 1);
                    cube.ChangeState(CubeState.Default);

                    break;
            }

        }
    }
    public void Focus(int colorID)
    {


        foreach (Cube cube in cubes)
        {

            if (cube.GetColorID() == colorID)
            {
                switch (cube.GetState())
                {
                    case CubeState.Colored:
                        break;
                    case CubeState.Default:
                        cube.ChangeState(CubeState.Focus);
                        break;
                    case CubeState.Focus:
                        break;
                    default:
                        MaterialManager.Ins.SetHighLightColor(cube);
                        cube.ChangeState(CubeState.Focus);
                        break;
                }
            }
            else
            {
                if (cube.IsState(CubeState.Focus))
                {
                    if (CameraManager.Ins.IsCameraState(CameraState.ZoomIn))
                    {
                        MaterialManager.Ins.SetShowTextColor(cube);
                        cube.ChangeState(CubeState.Zoomin);
                    }
                    if (CameraManager.Ins.IsCameraState(CameraState.ZoomOut))
                    {
                        cube.ChangeState(CubeState.Default);
                    }

                }
            }

        }
    }
    public void ReleaseFocusCube()
    {
        foreach(Cube cube in cubes)
        {
            if(cube.GetColorID() == currentColor && !cube.IsState(CubeState.Colored))
            {
                if (CameraManager.Ins.IsCameraState(CameraState.ZoomIn))
                {
                    MaterialManager.Ins.SetShowTextColor(cube);
                    cube.ChangeState(CubeState.Zoomin);
                }
                if (CameraManager.Ins.IsCameraState(CameraState.ZoomOut))
                {
                    cube.ChangeState(CubeState.Default);
                }
            }
        }
        currentColor = 0;
    }


}
[System.Serializable]
public class CubeType
{
    public int colorID;
    public int quantity;
    public int total;
    public CubeType(int colorID, int quantity,int total)
    {
        this.quantity = quantity;
        this.colorID = colorID;
        this.total = total;
    }   
}
