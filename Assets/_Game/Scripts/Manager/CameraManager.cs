using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;


public enum CameraState
{
    ZoomIn = 0,
    ZoomOut = 1,
}
public class CameraManager : Singleton<CameraManager>
{
    //public Material testMat;


    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion rotateOffset;
    [SerializeField] private Player player;
    [SerializeField] private float zoomDuration = 0.5f;
    public Camera cam;
    
    public Transform targetObject;
    
    public float zoomSpeed = 0.1f;
    public bool IsZooming = false;
    public float smoothy;
    private bool isZoomedIn = false;
    private bool isZoomedOut = true;
    private CameraState camState = CameraState.ZoomOut;

    [Header("ZoomInfo")]
    public float minZoom = 0.1f;
    public float maxZoom = 0.25f;
    public float checkPointZoom = 0.2f;

    public void SetZoomInfo(ZoomInfo zoomInfo)
    {
        this.minZoom = zoomInfo.minZoom;
        this.checkPointZoom = zoomInfo.checkPointZoom;
        this.maxZoom = zoomInfo.maxZoom;
        cam.orthographicSize = maxZoom;
    }
    private void Start()
    {
        
#if UNITY_EDITOR
        zoomSpeed = 10f;
#endif
    }
    private void Update()
    {
      

        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.touchCount == 2 && IsZooming)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);


            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;


            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            //float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff, zoomSpeed);

        }
        else
        {
            IsZooming = false;
        }

#if UNITY_EDITOR
        if (scrollInput != 0)
        {
            Zoom(-scrollInput, zoomSpeed);
          
        }
#endif
       
    }
    public bool IsCameraState(CameraState cs)
    {
        return this.camState == cs;
    }
    public void Reset()
    {
        cam.orthographicSize = maxZoom;
        camState = CameraState.ZoomOut;
    }
    private void Zoom(float deltaMagnitudeDiff, float speed)
    {
        IsZooming = true;
        cam.orthographicSize += Mathf.Log(Mathf.Abs(deltaMagnitudeDiff) + 1) * Mathf.Sign(deltaMagnitudeDiff) * speed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        MaterialManager.Ins.ChangeColorStateByFOV((cam.orthographicSize - checkPointZoom) / (maxZoom - checkPointZoom));

        if (cam.orthographicSize > checkPointZoom )
        {
            camState = CameraState.ZoomOut;
            UIManager.Ins.GetUI<UIGameplay>().ChangeZoomButtonState(camState);
        }
        else if (cam.orthographicSize < checkPointZoom )
        {
            camState = CameraState.ZoomIn;
            UIManager.Ins.GetUI<UIGameplay>().ChangeZoomButtonState(camState);
        }

    }
    public void SetFieldOfView()
    {
        //StartCoroutine(StartSetFOV());
        cam.DOOrthoSize(checkPointZoom, 2f);
     
    }
    public void ChangeZoomState()
    {
        switch (camState)
        {
            case CameraState.ZoomIn:
                camState = CameraState.ZoomOut;
                LerpDeCreaseOrthoSize(1f);
                cam.DOOrthoSize(maxZoom, zoomDuration);
                break;
            case CameraState.ZoomOut:
                camState = CameraState.ZoomIn;
                LerpInCreaseOrthoSize(1f);
                cam.DOOrthoSize((minZoom + checkPointZoom) / 2, zoomDuration);
                break;
        }
        UIManager.Ins.GetUI<UIGameplay>().ChangeZoomButtonState(camState);
    }
    public void LerpDeCreaseOrthoSize(float duration) {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            MaterialManager.Ins.ChangeColorStateByFOV(time);    
        }
        MaterialManager.Ins.ChangeColorStateByFOV(1);
    }
    public void LerpInCreaseOrthoSize(float duration)
    {
        float time = duration;
        while (time>0)
        {
            time -= Time.deltaTime;
            MaterialManager.Ins.ChangeColorStateByFOV(time);
        }
        MaterialManager.Ins.ChangeColorStateByFOV(0);
    }




    public CameraState CameraState { get { return camState; } }


}
[System.Serializable]
public class ZoomInfo
{
    public float minZoom = 0.1f;
    public float maxZoom = 0.25f;
    public float checkPointZoom = 0.2f;
    public ZoomInfo(float minZoom = 0.1f,float maxZoom = 0.25f, float checkPointZoom = 0.2f)
    {
        this.minZoom = minZoom;
        this.maxZoom = maxZoom;
        this.checkPointZoom = checkPointZoom;
    }
}
