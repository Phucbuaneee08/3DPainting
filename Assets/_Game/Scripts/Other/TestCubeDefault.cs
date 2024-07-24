using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeDefault : MonoBehaviour
{
    // Start is called before the first frame update
    public void DisplayIdCubeDefault()
    {
        List<int> idCubeDefault = new List<int>();
        List<Cube> cubes = LevelManager.Ins.Cubes;
        foreach (Cube cube in cubes)
        {
            if (cube.IsState(CubeState.Default))
            {
                idCubeDefault.Add(cube.ID);
            }
        }
        if (idCubeDefault.Count > 0)
        {
            string idsString = string.Join(",", idCubeDefault);
            Debug.Log("Default Cube IDs: " + idsString);
        }
       

    }
}
