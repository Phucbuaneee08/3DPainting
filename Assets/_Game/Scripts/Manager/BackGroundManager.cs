using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundManager : Singleton<BackGroundManager>
{
    [SerializeField] private List<Image> images = new List<Image>();
    [SerializeField] private Image backGround;

    private Color initialColor;
    private bool initialColorSet = false; 

    private void Start()
    {
        initialColor = backGround.color;
        initialColorSet = true;
    }

    public void ChangeImgBG(int id)
    {
        if (id >= 0 && id < images.Count)
        {
            foreach (var img in images)
            {
                img.gameObject.SetActive(false);
            }
            images[id].gameObject.SetActive(true);
            backGround.sprite = images[id].sprite;
        }
    }

    public void ChangeColorBG(Color _color)
    {
        if (!initialColorSet)
        {
            initialColor = backGround.color;
            initialColorSet = true;
        }
        backGround.color = _color;
    }

    public void ChangeColorBGGradually(Color targetColor, float duration)
    {
        if (!initialColorSet)
        {
            initialColor = backGround.color;
            initialColorSet = true;
        }
        StartCoroutine(ChangeColorCoroutine(targetColor, duration));
    }

    private IEnumerator ChangeColorCoroutine(Color targetColor, float duration)
    {
        Color initialColor = backGround.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            backGround.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backGround.color = targetColor; 
    }

    public void ResetToInitialColor()
    {
        if (initialColorSet)
        {
            backGround.color = initialColor;
        }
    }
}
