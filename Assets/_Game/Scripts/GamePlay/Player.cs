using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum PlayerState
{
    Normal = 0,
    FillBooster = 1,
    ZoomBooster =2,
}
public class Player : GameUnit
{
    [SerializeField] Animator animator;
    [SerializeField] Camera cam;
    [SerializeField] private bool isHaveFillBooster = true;
    public float rotateSpeed = 10f;
    public float dragSpeed = 50f;
    private float x;
    private float y;
    public float zoomDistance =200f;
    private Vector3 targetPosition;

    private bool isLeftDragging;
    private bool isRightDragging;
    private bool isCanMove;
    private bool isCanFillColor;
    
    private const string VICTORY_ANIM = "Victory";
    public bool IsDragging = false;

    private Vector2 finger1Start, finger2Start;
    private Vector2 finger1Last, finger2Last;
    private Vector2 initialTouchDelta;

    private Cube cachedCube;
    private void Start()
    {
        animator.enabled = false;
    }
    private void Update()
    {
        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;


        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isLeftDragging = true;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isLeftDragging = false;
                    isCanMove = true;
                    isCanFillColor = true;
                    break;

                case TouchPhase.Moved:
                    x = touch.deltaPosition.x;
                    y = touch.deltaPosition.y;
                    break;
                case TouchPhase.Stationary:
                    x = 0;
                    y = 0;
                    break;
            }
        }
        else if (Input.touchCount == 2 && !CameraManager.Ins.IsZooming)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                finger1Start = touch1.position;
                finger2Start = touch2.position;
                initialTouchDelta = finger2Start - finger1Start;

            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                isLeftDragging = false;
                isRightDragging = true;
                Vector2 finger1Move = touch1.deltaPosition;
                Vector2 finger2Move = touch2.deltaPosition;
                Vector2 finger1End = touch1.position;
                Vector2 finger2End = touch2.position;


                Vector2 currentTouchDelta = finger2End - finger1End;
                if (Mathf.Abs(currentTouchDelta.magnitude - initialTouchDelta.magnitude) > zoomDistance)
                {
                    CameraManager.Ins.IsZooming = true;
                    isRightDragging = false;
                }
                else
                {
                    Vector2 averageMove = (finger1Move + finger2Move) * Time.deltaTime;

                    x = averageMove.x;
                    y = averageMove.y;
                }
            }
            else if (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled ||
                        touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
            {
                // Trường hợp một trong hai ngón tay rời khỏi màn hình
                IsDragging = false;             
                isRightDragging = false;
                x = 0;
                y = 0;
            }
            else if (touch1.phase == TouchPhase.Stationary || touch2.phase == TouchPhase.Stationary)
            {
                // Trường hợp 2 ngón tay vẫn đặt trên màn hình nhưng không di chuyển
                IsDragging = false;
                isRightDragging = false;
                x = 0;
                y = 0;
            }
        }




#if UNITY_EDITOR
        rotateSpeed = 500f;
        if (Input.GetMouseButtonDown(0))
        {
           
            isLeftDragging = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            isRightDragging = true;
            IsDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isLeftDragging = false;
            isCanMove = true;
            isCanFillColor = true;
            isHaveFillBooster = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRightDragging = false;
          
        }

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
#endif
        if (isLeftDragging)
        {
            if (UIManager.Ins.GetUI<UIGameplay>().CheckInputOnUI()) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.CompareTag("Cube")) 
                { 
                  
                    Cube cube = Cache.GetCube(hitInfo.collider);
                    if ( BoosterManager.Ins.CheckBoosterQuantity() && !cube.IsState(CubeState.Colored) && isCanFillColor)
                    {
                        isCanMove = false;
                        BoosterManager.Ins.FillBoosterByColor(cube);
                    }
                    if (BoosterManager.Ins.CheckBooterFillByNumber() && !cube.IsState(CubeState.Colored) && isCanFillColor)
                    {
                        isCanMove = false;
                        BoosterManager.Ins.BoosterFillByNumber(cube);
                    }
                    if (cube != null && !cube.IsState(CubeState.Colored) && isCanFillColor)
                    {
                        if (cube.GetColorID() == LevelManager.Ins.currentColor)
                        {
                            isCanMove = false;
                            LevelManager.Ins.OnFilledCube(cube);
                        }
                        else if (isCanMove) isCanFillColor = false;


                    }
                    else if (isCanMove)
                    {
                        isCanFillColor = false;
                        if (OnCheckIsTouching(0.01f))
                        {
                            OnRotate();
                        }
                    }
                }


            }
            else if (isCanMove)
            {
                isCanFillColor = false;
                if (OnCheckIsTouching(0.01f) && isCanMove)
                {
                    OnRotate();
                }
            }

        }

        if (isRightDragging)
        {
            OnMoving();
        }


    }
    public void OnMoving()
    {
        Vector3 screenPosition = cam.WorldToScreenPoint(transform.position);
        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

        Vector3 moveDirection = new Vector3(x, y, 0f) * dragSpeed;
        Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition + moveDirection);

        transform.position = worldPosition;
    }
    public void OnRotate()
    {
        transform.Rotate(new Vector3(0, -x * rotateSpeed * Time.deltaTime, 0));
        transform.Rotate(new Vector3(y * rotateSpeed * Time.deltaTime, 0, 0), Space.World);
    }
    public bool OnCheckIsTouching(float limit)
    {
        return Mathf.Abs(x) > limit || Mathf.Abs(y) > limit;
    }


    public void OnReset()
    {
        animator.enabled = false;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        isLeftDragging = false;
        isRightDragging = false;
        isCanMove = false;
        isCanFillColor = false;
        
    }
    public void PlayAnim()
    {
        animator.enabled = true;
        animator.SetTrigger(VICTORY_ANIM);
    }
    public void MoveToStartPosition()
    {
        StartCoroutine(MovePlayer());
    }
    IEnumerator MovePlayer()
    {
        float moveDuration = 2f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;

        float timeElapsed = 0;

        while (timeElapsed < moveDuration)
        {

            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / moveDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
    public void ResetFillbooster()
    {
        isHaveFillBooster = true;
    }

}
