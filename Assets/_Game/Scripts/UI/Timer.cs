using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float remainingTime; 
    public Text timerText;
    private bool _isCanCountDown;

    void Update()
    {
        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;
        if (remainingTime > 0)
        {

            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            _isCanCountDown = false;
            LevelManager.Ins.CheckReviveOrFail();
            //UIManager.Ins.OpenUI<UIRevive>();
            timerText.text = "0:00";
        }
    }
    public void SetRemainTime(int time)
    {
        this.remainingTime = time;
        _isCanCountDown = true;
    }
}
