using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image imageSource;
    [SerializeField] private int levelID;
    [SerializeField] private Text text;
    private Level level;


    public void SetData(int levelID,Level level,Sprite avatar)
    {
        this.levelID = levelID;
        this.level = level;
        text.text = levelID.ToString();
        imageSource.sprite = avatar;

    }
    public void SelectLevel()
    {
        LevelManager.Ins.OnLoadLevel(level);
        UIManager.Ins.CloseUI<UIMainMenu>();
    }
   

}
