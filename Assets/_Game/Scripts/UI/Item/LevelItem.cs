using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    public Image imageSource;
    [SerializeField] private int levelID;
    private Level level;
    public Image img;
    [SerializeField] private Button button;
    public void SetData(int levelID, Level level, Sprite avatar, bool _isPassed,bool _isShowTextPassed, bool _isShowInter)
    {
        this.levelID = levelID;
        this.level = level;
        imageSource.sprite = avatar;
        img.gameObject.SetActive(_isPassed);
        button.interactable = _isShowInter; 
    }
    public void SelectLevel()
    {
        LevelManager.Ins.OnLoadLevel(levelID);
        UIManager.Ins.CloseUI<UIMainMenu>();
    }
    public int GetID()
    {
        return levelID;
    }
    public void SetColorImg()
    {
        Color originalColor = imageSource.color;
        Color grayscaleColor = Ultilities.ConvertToGrayscale(originalColor);
        imageSource.color = grayscaleColor;
    }
    
}
