using DG.Tweening;
using Paint3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public LevelDatas levelDatas;
    [SerializeField] private Player player;
    [SerializeField] private List<Level> levels;
    [SerializeField] private List<Cube> cubes;
    [SerializeField] private List<CubeType> cubeTypes;
    [SerializeField] private int totalTime;
    [SerializeField] private int reviveTime;
    [SerializeField] private GameObject root;
    public Vector3 rotateOffset;

    public long vibrationDuration = 500;

    private bool _isCanRevive;


    
    public Level currentLevel;
    public int currentColor;
    public int cubeTotal;
    public int currentlevelID;
    public List<Cube> Cubes => cubes;
    private AnimationGameUnit _currentAnim;
    public bool IsCanUseAds { get; set; }   





    public void OnInit()
    {
        IsCanUseAds = true;
        currentColor = 0;
        _isCanRevive = true;
        player.gameObject.SetActive(true);
        cubeTotal = currentLevel.cubes.Count;
        if (currentLevel.materials.Count > 0)
        {
            foreach (MaterialData material in currentLevel.materials)
            {
                int count = 0;
                foreach (CubeData cube in currentLevel.cubes)
                {
                    if (cube.realColorID == material.colorID)
                    {
                        count++;
                    }
                }
                cubeTypes.Add(new CubeType(material.colorID, count, count));
            }
        }
    }

   
    public void OnReset()
    {
        if (_currentAnim != null) Destroy(_currentAnim.gameObject);
        IsCanUseAds = true;
        root.SetActive(true);
        player.OnReset();
        cubeTypes.Clear();
        foreach (Cube cube in cubes)
        {
            SimplePool.Despawn(cube);
        }
        cubes.Clear();
        SimplePool.CollectAll();
        MaterialManager.Ins.OnResetDefaultColor();
        MaterialManager.Ins.ResetFloatShaderColor();
        UIManager.Ins.GetUI<UIGameplay>().ResetItem();
        //UIManager.Ins.GetUI<MainMenu>().ReLoadData();
        CameraManager.Ins.Reset();
        BoosterManager.Ins.ResetAllBooster();
        UIManager.Ins.CloseAll();
    }

   
    public void OnLoadLevel(int levelID)
    {
        currentLevel = levelDatas.GetLevelWithID(levelID).level;
        //currentLevel = levelDatas.level3D[levelID - 1].level;
        //if (levelID > DataManager.Ins.playerData.currentlevelID)
        DataManager.Ins.playerData.currentlevelID = levelID;
        OnInit();
        CameraManager.Ins.SetZoomInfo(currentLevel.zoomInfo);
        MaterialManager.Ins.SetMatData(currentLevel.materials);
     
        for (int i = 0; i < currentLevel.cubes.Count; i++)
        {
            Cube newCube = SimplePool.Spawn<Cube>(PoolType.Cube, currentLevel.cubes[i].position, Quaternion.identity);
            newCube.SetCubeData(i, currentLevel.cubes[i].realColorID, currentLevel.cubes[i].defaultColorID);        
            MaterialManager.Ins.SetDefaultShaderColor(newCube, newCube.GetColorID() - 1);

            cubes.Add(newCube);

        }

        //player.transform.DORotate(rotateOffset, 0f);
    
        player.transform.DORotate(rotateOffset, 0f);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIGameplay>().InitColorItem(currentLevel.materials);
        UIManager.Ins.GetUI<UIGameplay>().SetCountDownTime(totalTime);
        BoosterManager.Ins.OnInit();


    }
    public void NextLevel()
    {
        OnReset();
        OnLoadLevel(DataManager.Ins.playerData.currentlevelID);
    }
    public void OnFilledCube(Cube cube)
    {
        if (cube.IsState(CubeState.Colored)) return;

        ParticlePool.Play(ParticleType.Explosion,cube.transform.position); // hieu ung cube 
        AudioManager.Ins.OnFilledCube();

//#if UNITY_EDITOR
//        cubes.Remove(cube);
//#endif

        cube.ChangeState(CubeState.Colored);
        MaterialManager.Ins.SetColor(cube, cube.GetColorID());
        RemoveCubeByColorID(cube.GetColorID());
        if (cubeTotal == 0)
        {
            //UIManager.Ins.OpenUI<UIVictory>();
            StartCoroutine(OnCelebration());
        }

        Vibration.Vibrate(vibrationDuration); // rung man hinh 

    }
    public void RemoveCubeByColorID(int colorID)
    {

        foreach (CubeType cubes in cubeTypes)
        {
            if (cubes.colorID == colorID)
            {

                cubes.quantity--;
                cubeTotal--;
                StartCoroutine(OnRemoveCube(cubes));
                if (cubes.quantity == 0)
                {
                    if (cubeTypes.Count > 0 && cubeTotal > 0 && !BoosterManager.Ins.CheckBoosterQuantity())
                    {
                        CubeType currentColorType = Ultilities.CheckNextCubeTypeInList(cubeTypes, currentColor);
                        currentColor = currentColorType.colorID;

                        ItemManager.Ins.TurnOnColorItemByColorID(currentColor);
                    }
                    UIManager.Ins.GetUI<UIGameplay>().RemoveColorItem(colorID);
                }
            }
        }
    }
    private IEnumerator OnRemoveCube(CubeType cubes)
    {
        yield return null;
        ColorItem ci = UIManager.Ins.GetUI<UIGameplay>().FindItemByColorId(cubes.colorID);
        if (ci != null)
            ci.SetFillAmount((float)cubes.quantity / (float)cubes.total);
    }
    private IEnumerator OnCelebration()
    {
        //player.PlayAnim();
        GameManager.Ins.ChangeState(GameState.Finish);
        CameraManager.Ins.SetFieldOfView();
        UIManager.Ins.CloseAll();
        Color lightPink = new Color(1f, 0.71f, 0.76f);
        //BackGroundManager.Ins.ChangeColorBGGradually(lightPink, 2f);
        player.MoveToStartPosition(Vector3.zero, rotateOffset);
        yield return new WaitForSeconds(2f);

        if (SimplePool.FindPrefabByType(currentLevel.poolType))
        {

            root.SetActive(false);
            _currentAnim = SimplePool.Spawn<AnimationGameUnit>(currentLevel.poolType,player.transform);

        }
        SaveLevelData();
        yield return new WaitForSeconds(2f);
        Victory();
    }

    // tăng số level mà người chơi đã hoàn thành trong data

    public void SaveLevelData()
    {
        PlayerData playerData = DataManager.Ins.playerData;

        playerData.GetDataWithID(DataManager.Ins.playerData.currentlevelID).isColored = true;
        playerData.CountLevelPassed += 1;
        if(DataManager.Ins.playerData.isSpinReward == 0)
        {
            playerData.countProgresses += 1;
            if (playerData.countProgresses == 10)
            {
                playerData.isSpinReward = 1;
            }
        }
        DataManager.Ins.SaveData();
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
        if (_isCanRevive)
        {
            _isCanRevive = false;
            if (UIManager.Ins.IsLoaded<UIGameplay>())
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
        OnReset();
        UIManager.Ins.OpenUI<MainMenu>();
        //UIManager.Ins.OpenUI<UIMainMenu>();
    }
  

   
    public void FocusByColorId(int colorID)
    {
        if(currentColor!=0)
                MaterialManager.Ins.SetShowTextShaderColor(currentColor);

   
        MaterialManager.Ins.SetHightLigtShaderColor(colorID);
        currentColor = colorID;

    }
    public void ReleaseFocusCube()
    {

        if (currentColor != 0)
        { 
            MaterialManager.Ins.SetShowTextShaderColor(currentColor);
            currentColor = 0;
        }
  
      
    }


}
[System.Serializable]
public class CubeType
{
    public int colorID;
    public int quantity;
    public int total;
    public CubeType(int colorID, int quantity, int total)
    {
        this.quantity = quantity;
        this.colorID = colorID;
        this.total = total;
    }
}
