
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState { GamePlay, MainMenu, Finish, Revive, Setting,Pause,ShowPassedLevel }


public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.MainMenu;
    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState) => this.gameState == gameState;

    private void Awake()
    {
      
        Application.targetFrameRate = 1200;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        StartCoroutine(I_InitGame());
    }
    IEnumerator I_InitGame()
    {
        /*yield return new WaitUntil(
            () => (
            Ins != null
            && DataManager.Ins != null
            && UIManager.Ins != null
            && LevelManager.Ins != null
            )
        );*/
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<Loading>();
        DataManager.Ins.LoadData();
    }
}