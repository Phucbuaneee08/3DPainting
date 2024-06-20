
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState { GamePlay, MainMenu, Finish, Revive, Setting }


public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState) => this.gameState == gameState;

    private void Awake()
    {
      
    }

    private void Start()
    {

        UIManager.Ins.OpenUI<UIMainMenu>();
    }

}