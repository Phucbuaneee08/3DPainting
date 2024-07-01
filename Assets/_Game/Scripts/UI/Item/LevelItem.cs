using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    public Image imageSource;
    [SerializeField] private int levelID;
    [SerializeField] private Text text;
    private Level level;
    public Image img;
    public void SetData(int levelID, Level level, Sprite avatar, bool _isPassed)
    {
        this.levelID = levelID;
        this.level = level;
        text.text = levelID.ToString();
        imageSource.sprite = avatar;
        img.gameObject.SetActive(_isPassed);
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
    IEnumerator IE_SetColorImg()
    {
        yield return new WaitForEndOfFrame();
    }
}
