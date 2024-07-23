using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Paint3D;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;
    Image backgroundImage, handleImage;
    Color backgroundDefaultColor, handleDefaultColor;
    Toggle toggle;
    Vector2 handlePosition;

    public enum ToggleType
    {
        Music,
        Sound,
        Vibration
    }

    [SerializeField] ToggleType toggleType; 

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = uiHandleRectTransform.anchoredPosition;
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();
        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;
        toggle.onValueChanged.AddListener(OnSwitch);

        switch (toggleType)
        {
            case ToggleType.Music:
                toggle.isOn = DataManager.Ins.playerData.music;
                break;
            case ToggleType.Sound:
                toggle.isOn = DataManager.Ins.playerData.sound;
                break;
            case ToggleType.Vibration:
                toggle.isOn = DataManager.Ins.playerData.vibrationEnabled;
                break;
        }

        OnSwitch(toggle.isOn);
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
        handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, .4f);

        switch (toggleType)
        {
            case ToggleType.Music:
                if (on)
                    AudioManager.Ins.ResumeMusic();
                else
                    AudioManager.Ins.MuteMusic();
                break;
            case ToggleType.Sound:
                if (on)
                    AudioManager.Ins.ResumeSound();
                else
                    AudioManager.Ins.MuteSound();
                break;
            case ToggleType.Vibration:
                DataManager.Ins.playerData.vibrationEnabled = on;
                break;
        }
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
