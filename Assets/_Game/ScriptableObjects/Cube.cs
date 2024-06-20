using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum CubeState
{
    Default = 0,
    Zoomin = 1,
    Focus = 2,
    Colored = 3,

}

public class Cube : GameUnit
{
    public Renderer colorRender;
    [SerializeField] private int id;
    [SerializeField] private int colorID;
    [SerializeField] private int defaultColorID;
    [SerializeField] private CubeState cubeState;
    private void Start()
    {
        colorRender = GetComponent<MeshRenderer>();
    }
    public void SetCubeData(int id, int colorID, int defaultColorID)
    {
        this.id = id;
        this.colorID = colorID;
        this.defaultColorID = defaultColorID;
        cubeState = CubeState.Default;
    }

    public bool IsState(CubeState state)
    {
        return this.cubeState == state;
    }
    public CubeState GetState()
    {
        return cubeState;
    }
    public void ChangeState(CubeState state)
    {
        this.cubeState = state;
    }
    public int GetColorID()
    {
        return colorID;
    }
    public int GetDefaultColorID()
    {
        return defaultColorID;
    }
}
