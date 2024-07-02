using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
    private void Awake()
    {
    }
    public void Start()
    {

    }
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
        //ReLoadData();
    }
    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }
    private void Update()
    {
        
    }
}
