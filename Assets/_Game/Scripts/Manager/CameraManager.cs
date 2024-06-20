using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public enum CameraState
{
    ZoomIn = 0,
    ZoomOut = 1,
}
public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion rotateOffset;
    public Camera cam;
    public Transform targetObject; 
    public float zoomSpeed = 0.01f;
    public bool IsZooming;
    public float dragSpeed = 2;
    public float smoothy;
    private bool isZoomedIn = false;
    private bool isZoomedOut = true;
    private CameraState camState = CameraState.ZoomOut;

    [Header("ZoomInfo")]
    public float minZoom = 20.0f;
    public float maxZoom = 60.0f;
    public float checkPointZoom = 50f;

    public void SetZoomInfo(ZoomInfo zoomInfo)
    {
        this.minZoom = zoomInfo.minZoom;
        this.checkPointZoom = zoomInfo.checkPointZoom;
        this.maxZoom = zoomInfo.maxZoom;
        cam.fieldOfView = maxZoom;
    }
    private void Start()
    {
        cam.transform.position = targetObject.transform.position+ offset;
        cam.transform.rotation = Quaternion.Lerp(transform.rotation, rotateOffset, 1);
    }
    private void Update()
    {
    
        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);


            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;


            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            //float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            cam.fieldOfView += Mathf.Log(Mathf.Abs(deltaMagnitudeDiff) + 1) * Mathf.Sign(deltaMagnitudeDiff) * zoomSpeed;

            // Giới hạn giá trị zoom của camera
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
       
        }
        else
        {
            IsZooming = false;
        }

#if UNITY_EDITOR
        if (scrollInput != 0)
        {
            Zoom(-scrollInput, zoomSpeed);
            //cam.fieldOfView -= scrollInput * zoomSpeed;
            //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
#endif
        if (cam.fieldOfView > checkPointZoom && !isZoomedOut)
        {
            Debug.Log("!!!ZoomOut");
            LevelManager.Ins.Zoomout();
            isZoomedOut = true;
            isZoomedIn = false;
            camState = CameraState.ZoomOut;
        }
        else if (cam.fieldOfView < checkPointZoom && !isZoomedIn)
        {
            Debug.Log("!!!ZoomIn");
            LevelManager.Ins.Zoomin();
            isZoomedIn = true;
            isZoomedOut = false;
            camState = CameraState.ZoomIn;
        }
    }
    public bool IsCameraState(CameraState cs)
    {
        return this.camState == cs;
    }
    public void Reset()
    {
        cam.fieldOfView = 60f;
        camState = CameraState.ZoomOut;
    }
    private void Zoom(float deltaMagnitudeDiff, float speed)
    {
        IsZooming = true;
        cam.fieldOfView += Mathf.Log(Mathf.Abs(deltaMagnitudeDiff) + 1) * Mathf.Sign(deltaMagnitudeDiff) * speed;
        //cam.fieldOfView += deltaMagnitudeDiff * speed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
    } 

    private IEnumerator StartSetFOV()
    {
        float duration = 1f;
        float elapsedTime = 0f;
        float initialFOV = cam.fieldOfView;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t);
            cam.fieldOfView = Mathf.Lerp(initialFOV, checkPointZoom, t);
            yield return null;
        }

        
        cam.fieldOfView = checkPointZoom;
     
    }
    public void SetFieldOfView()
    {
        StartCoroutine(StartSetFOV());
      
    }
    

}
[System.Serializable]
public class ZoomInfo
{
    public float minZoom = 5;
    public float maxZoom = 60;
    public float checkPointZoom = 50;
    public ZoomInfo(float minZoom = 5,float maxZoom = 60, float checkPointZoom = 50)
    {
        this.minZoom = minZoom;
        this.maxZoom = maxZoom;
        this.checkPointZoom = checkPointZoom;
    }
}
