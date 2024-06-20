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
                cubeTypes.Add(new CubeType(material.colorID, count));
            }
        }
    }

    public void FocusByColorID(int colorID)
    {
        Focus(colorID);
        this.currentColor = colorID;
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
        MaterialManager.Ins.SetColor(cube, cube.GetColorID());
        cube.ChangeState(CubeState.Colored);
        //ParticlePool.Play(ParticleType.Hit_1, cube.TF);
        RemoveCubeByColorID(cube.GetColorID());
        if(cubeTotal==0)
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
                if (cubes.quantity == 1)
                {
                    UIManager.Ins.GetUI<UIGameplay>().RemoveColorItem(colorID);
                }
                cubes.quantity--;
                cubeTotal--;
            }
        }
        
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


}
[System.Serializable]
public class CubeType
{
    public int colorID;
    public int quantity;
    
    public CubeType(int colorID, int quantity)
    {
        this.quantity = quantity;
        this.colorID = colorID;
    }   
}
