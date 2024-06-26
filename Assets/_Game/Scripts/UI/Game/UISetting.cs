using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UICanvas
{
    public InputField zoomSpeed;
    public InputField dragSpeed;
    public InputField rotateSpeed;
    public InputField moveDistance;
    public override void Setup()
    {
        base.Setup();
     
    }
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }
    public void Home()
    {
        LevelManager.Ins.Home();
    }
    public override void CloseDirectly()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        base.CloseDirectly();
    }
    public void ChangeZoomSpeed()
    {
        if (float.TryParse(zoomSpeed.text, out float newZoomSpeed))
        {
            CameraManager.Ins.zoomSpeed = newZoomSpeed;
            Debug.Log("Zoom speed changed to: " + newZoomSpeed);
        }
        else
        {
            Debug.LogWarning("Invalid input for zoom speed.");
        }
    }

    public void ChangeDragSpeed()
    {
        if (float.TryParse(dragSpeed.text, out float newDragSpeed))
        {
            FindObjectOfType<Player>().dragSpeed = newDragSpeed;
            Debug.Log("Drag speed changed to: " + newDragSpeed);
        }
        else
        {
            Debug.LogWarning("Invalid input for drag speed.");
        }
    }

    public void ChangeRotateSpeed()
    {
        if (float.TryParse(rotateSpeed.text, out float newRotateSpeed))
        {
            FindObjectOfType<Player>().rotateSpeed = newRotateSpeed;
            Debug.Log("Rotate speed changed to: " + newRotateSpeed);
        }
        else
        {
            Debug.LogWarning("Invalid input for rotate speed.");
        }
    }
    public void SetMoveDistance()
    {
        if (float.TryParse(moveDistance.text, out float moveDistancee))
        {
            FindObjectOfType<Player>().zoomDistance = moveDistancee;
            Debug.Log("Rotate speed changed to: " + moveDistance);
        }
        else
        {
            Debug.LogWarning("Invalid input for rotate speed.");
        }
    }
}
